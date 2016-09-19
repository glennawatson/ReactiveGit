namespace ReactiveGit.Gui.Base.ViewModel.Branches
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Text;
    using System.Windows.Input;

    using ReactiveGit.ExtensionMethods;
    using ReactiveGit.Gui.Base.ExtensionMethods;
    using ReactiveGit.Gui.Base.Model;
    using ReactiveGit.Gui.Base.Model.Branches;
    using ReactiveGit.Model;

    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;

    /// <summary>
    /// View model dealing with branch related issues.
    /// </summary>
    public class BranchViewModel : ReactiveObject, IBranchViewModel
    {
        private readonly ReactiveCommand<GitBranch, Unit> checkoutBranch;

        private readonly ObservableAsPropertyHelper<GitBranch> checkedOutBranch;

        private readonly ReactiveCommand<Unit, GitBranch> getBranches;

        private readonly IRepositoryDetails repositoryDetails;

        private readonly IReactiveList<BranchNode> branches = new ReactiveList<BranchNode>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchViewModel"/> class.
        /// </summary>
        /// <param name="repositoryDetails">The details about the repository.</param>
        public BranchViewModel(IRepositoryDetails repositoryDetails)
        {
            if (repositoryDetails == null)
            {
                throw new ArgumentNullException(nameof(repositoryDetails));
            }

            this.repositoryDetails = repositoryDetails;

            this.getBranches = ReactiveCommand.CreateFromObservable(this.GetBranchesImpl);
            this.getBranches.Subscribe(this.AddNode);

            this.checkedOutBranch = this.repositoryDetails.BranchManager.CurrentBranch.ToProperty(this, x => x.CheckedOutBranch, out this.checkedOutBranch);

            IObservable<bool> isValidBranch = this.WhenAnyValue(x => x.CurrentBranch).Select(x => x != null);
            this.checkoutBranch = ReactiveCommand.CreateFromObservable<GitBranch, Unit>(x => repositoryDetails.BranchManager.CheckoutBranch(x), isValidBranch);

            this.Refresh.InvokeCommand();
        }

        /// <summary>
        /// Gets the current branch.
        /// </summary>
        public GitBranch CheckedOutBranch => this.checkedOutBranch.Value;

        /// <inheritdoc />
        [Reactive]
        public GitBranch CurrentBranch { get; set; }

        /// <inheritdoc />
        public IEnumerable<BranchNode> Branches => this.branches;

        /// <inheritdoc />
        public ICommand Refresh => this.getBranches;

        /// <inheritdoc />
        public ICommand CheckoutBranch => this.checkoutBranch;

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

                int index = currentLevel.BinarySearchIndexOfBy(
                    (compareNode, name) => comparer.Compare(compareNode.Name, name), currentNodeName);

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
            return this.repositoryDetails.BranchManager.GetLocalBranches();
        }
    }
}