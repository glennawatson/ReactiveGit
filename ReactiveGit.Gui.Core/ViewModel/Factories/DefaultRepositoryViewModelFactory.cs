// <copyright file="DefaultRepositoryViewModelFactory.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.Core.ViewModel.Factories
{
    using ReactiveGit.Gui.Core.Model;
    using ReactiveGit.Gui.Core.ViewModel.Branches;
    using ReactiveGit.Gui.Core.ViewModel.CommitHistory;
    using ReactiveGit.Gui.Core.ViewModel.RefLog;
    using ReactiveGit.Gui.Core.ViewModel.Repository;

    /// <summary>
    /// The default factory for generating repository view models.
    /// </summary>
    public class DefaultRepositoryViewModelFactory : IRepositoryViewModelFactory
    {
        /// <inheritdoc />
        public IBranchViewModel CreateBranchViewModel()
        {
            return new BranchViewModel();
        }

        /// <inheritdoc />
        public ICommitHistoryViewModel CreateCommitHistoryViewModel(IRepositoryDetails repositoryDetails)
        {
            return new CommitHistoryViewModel(repositoryDetails);
        }

        /// <inheritdoc />
        public IRefLogViewModel CreateRefLogViewModel()
        {
            return new RefLogViewModel();
        }

        /// <inheritdoc />
        public IRepositoryDocumentViewModel CreateRepositoryViewModel(IRepositoryDetails repositoryDetails)
        {
            return new RepositoryDocumentViewModel(this, repositoryDetails);
        }
    }
}