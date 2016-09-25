namespace ReactiveGit.Gui.Core.ViewModel.Branches
{
    using System;
    using System.Collections.Generic;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Windows.Input;

    using ReactiveGit.Core.ExtensionMethods;
    using ReactiveGit.Core.Model;
    using ReactiveGit.Gui.Core.ExtensionMethods;
    using ReactiveGit.Gui.Core.Model;
    using ReactiveGit.Gui.Core.Model.Branches;

    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;

    /// <summary>
    /// View model dealing with branch related issues.
    /// </summary>
    public class BranchViewModel : ReactiveObject, IBranchViewModel
    {
        private readonly IReactiveList<BranchNode> branches = new ReactiveList<BranchNode>();

        private readonly ReactiveCommand<GitBranch, Unit> checkoutBranch;

        private readonly ReactiveCommand<Unit, GitBranch> getBranches;

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchViewModel"/> class.
        /// </summary>
        public BranchViewModel()
        {
            IObservable<bool> isValidRepository = this.WhenAnyValue(x => x.RepositoryDetails).Select(x => x != null);

            this.getBranches = ReactiveCommand.CreateFromObservable(this.GetBranchesImpl, isValidRepository);
            this.getBranches.Subscribe(this.AddNode);

            IObservable<bool> isValidBranch =
                this.WhenAnyValue(x => x.CurrentBranch, x => x.RepositoryDetails).Select(
                    x => x.Item1 != null && x.Item2 != null);

            this.checkoutBranch =
                ReactiveCommand.CreateFromObservable<GitBranch, Unit>(
                    x => this.RepositoryDetails.BranchManager.CheckoutBranch(x),
                    isValidBranch);

            this.Refresh.InvokeCommand();
        }

        /// <inheritdoc />
        public IEnumerable<BranchNode> Branches => this.branches;

        /// <inheritdoc />
        public ICommand CheckoutBranch => this.checkoutBranch;

        /// <inheritdoc />
        [Reactive]
        public GitBranch CurrentBranch { get; set; }

        /// <inheritdoc />
        public string Name => "Branches";

        /// <inheritdoc />
        public ICommand Refresh => this.getBranches;

        /// <inheritdoc />
        [Reactive]
        public IRepositoryDetails RepositoryDetails { get; set; }

        private void AddNode(GitBranch gitBranch)
        {
            if (gitBranch == null)
            {
                return;
            }

            string[] nodeNames = gitBranch.FriendlyName.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var comparer = StringComparer.CurrentCultureIgnoreCase;

            IList<BranchNode> currentLevel = this.branches;
            for (int i = 0; i < nodeNames.Length; ++i)
            {
                bool isLast = i == nodeNames.Length - 1;

                string currentNodeName = nodeNames[i];

                int index =
                    currentLevel.BinarySearchIndexOfBy(
                        (compareNode, name) => comparer.Compare(compareNode.Name, name),
                        currentNodeName);

                if (index >= 0 && isLast)
                {
                    throw new Exception($"There is a duplicate leaf of name {gitBranch.FriendlyName}");
                }

                if (isLast)
                {
                    currentLevel.Add(new BranchLeaf(currentNodeName, gitBranch));
                    return;
                }

                BranchNode node = null;
                if (index >= 0)
                {
                    node = currentLevel[index];
                }

                if (node is BranchLeaf)
                {
                    throw new Exception($"There is a leaf node with the same name as a parent {gitBranch.FriendlyName}");
                }

                BranchParent parent = node as BranchParent;

                if (parent == null)
                {
                    var fullName = string.Join("/", nodeNames, 0, i + 1);

                    parent = new BranchParent(currentNodeName, fullName);

                    if (index >= 0)
                    {
                        throw new Exception($"There is a duplicate node of name {gitBranch.FriendlyName}");
                    }

                    currentLevel.Insert(~index, parent);
                }

                currentLevel = parent.ChildNodes;
            }
        }

        private IObservable<GitBranch> GetBranchesImpl()
        {
            this.branches.Clear();
            return this.RepositoryDetails.BranchManager.GetLocalBranches();
        }
    }
}