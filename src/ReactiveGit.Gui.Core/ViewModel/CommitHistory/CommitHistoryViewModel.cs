// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData.Binding;
using ReactiveGit.Gui.Core.ExtensionMethods;
using ReactiveGit.Gui.Core.Model;
using ReactiveGit.Gui.Core.ViewModel.GitObject;
using ReactiveGit.Library.Core.Model;
using ReactiveUI;

namespace ReactiveGit.Gui.Core.ViewModel.CommitHistory
{
    /// <summary>
    /// A view model related to a repository.
    /// </summary>
    public class CommitHistoryViewModel : GitObjectViewModelBase, ICommitHistoryViewModel
    {
        private readonly ObservableCollectionExtended<CommitHistoryItemViewModel> _commitHistory =
            new ObservableCollectionExtended<CommitHistoryItemViewModel>();

        private readonly ReactiveCommand<Unit, GitCommit> _refresh;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommitHistoryViewModel" /> class.
        /// </summary>
        /// <param name="repositoryDetails">The details of the repository.</param>
        /// <exception cref="ArgumentException">If the repository does not exist.</exception>
        public CommitHistoryViewModel(IRepositoryDetails repositoryDetails)
        {
            if (repositoryDetails == null)
            {
                throw new ArgumentNullException(nameof(repositoryDetails));
            }

            RepositoryDetails = repositoryDetails;
            IObservable<bool> isCurrentBranchObservable =
                this.WhenAnyValue(x => x.RepositoryDetails.SelectedBranch).Select(x => x != null);

            _refresh =
                ReactiveCommand.CreateFromObservable(
                    () => GetCommitsImpl(RepositoryDetails.SelectedBranch),
                    isCurrentBranchObservable);
            _refresh.Subscribe(x => _commitHistory.Add(new CommitHistoryItemViewModel(x)));

            isCurrentBranchObservable.Subscribe(x => Refresh.InvokeCommand());
        }

        /// <inheritdoc />
        public IEnumerable<CommitHistoryItemViewModel> CommitHistory => _commitHistory;

        /// <inheritdoc />
        public override string FriendlyName => "Commit History";

        /// <summary>
        /// Gets a command that will refresh the display.
        /// </summary>
        public override ICommand Refresh => _refresh;

        private IObservable<GitCommit> GetCommitsImpl(GitBranch branch)
        {
            _commitHistory.Clear();
            return RepositoryDetails.BranchManager.GetCommitsForBranch(branch, 0, 0, GitLogOptions.IncludeMerges);
        }
    }
}