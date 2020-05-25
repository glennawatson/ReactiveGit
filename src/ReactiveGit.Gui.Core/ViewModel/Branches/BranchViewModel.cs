// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData.Binding;
using ReactiveGit.Gui.Core.Model;
using ReactiveGit.Gui.Core.Model.Branches;
using ReactiveGit.Gui.Core.ViewModel.Content;
using ReactiveGit.Library.Core.ExtensionMethods;
using ReactiveGit.Library.Core.Model;
using ReactiveUI;

namespace ReactiveGit.Gui.Core.ViewModel.Branches
{
    /// <summary>
    /// View model dealing with branch related issues.
    /// </summary>
    public class BranchViewModel : ContentViewModelBase, IBranchViewModel
    {
        private readonly ObservableCollectionExtended<BranchNode> _branches = new ObservableCollectionExtended<BranchNode>();

        private readonly ReactiveCommand<GitBranch, Unit> _checkoutBranch;

        private readonly ReactiveCommand<Unit, GitBranch> _getBranches;
        private IRepositoryDetails _repositoryDetails;

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchViewModel" /> class.
        /// </summary>
        public BranchViewModel()
        {
            IObservable<bool> isValidRepository = this.WhenAnyValue(x => x.RepositoryDetails).Select(x => x != null);

            _getBranches = ReactiveCommand.CreateFromObservable(GetBranchesImpl, isValidRepository);
            _getBranches.Subscribe(AddNode);

            IObservable<bool> isValidBranch =
                this.WhenAnyValue(x => x.RepositoryDetails.SelectedBranch, x => x.RepositoryDetails).Select(
                    x => (x.Item1 != null) && (x.Item2 != null));

            _checkoutBranch =
                ReactiveCommand.CreateFromObservable<GitBranch, Unit>(
                    x => RepositoryDetails.BranchManager.CheckoutBranch(x),
                    isValidBranch);
        }

        /// <inheritdoc />
        public IEnumerable<BranchNode> Branches => _branches;

        /// <inheritdoc />
        public ICommand CheckoutBranch => _checkoutBranch;

        /// <inheritdoc />
        public override string FriendlyName => "Branches";

        /// <inheritdoc />
        public ICommand Refresh => _getBranches;

        /// <inheritdoc />
        public IRepositoryDetails RepositoryDetails
        {
            get => _repositoryDetails;
            set => this.RaiseAndSetIfChanged(ref _repositoryDetails, value);
        }

        private void AddNode(GitBranch gitBranch)
        {
            if (gitBranch == null)
            {
                return;
            }

            string[] nodeNames = gitBranch.FriendlyName.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            StringComparer comparer = StringComparer.CurrentCultureIgnoreCase;

            IList<BranchNode> currentLevel = _branches;
            for (var i = 0; i < nodeNames.Length; ++i)
            {
                bool isLast = i == nodeNames.Length - 1;

                string currentNodeName = nodeNames[i];

                int index =
                    currentLevel.BinarySearchIndexOfBy(
                        (compareNode, name) => comparer.Compare(compareNode.Name, name),
                        currentNodeName);

                if ((index >= 0) && isLast)
                {
                    throw new Exception($"There is a duplicate leaf of name {gitBranch.FriendlyName}");
                }

                if (isLast)
                {
                    currentLevel.Add(new BranchLeaf(gitBranch));
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

                var parent = node as BranchParent;

                if (parent == null)
                {
                    string fullName = string.Join("/", nodeNames, 0, i + 1);

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
            _branches.Clear();
            return RepositoryDetails.BranchManager.GetLocalBranches();
        }
    }
}