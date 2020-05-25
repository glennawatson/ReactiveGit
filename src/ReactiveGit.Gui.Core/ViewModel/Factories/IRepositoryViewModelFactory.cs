// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveGit.Gui.Core.Model;
using ReactiveGit.Gui.Core.ViewModel.Branches;
using ReactiveGit.Gui.Core.ViewModel.CommitHistory;
using ReactiveGit.Gui.Core.ViewModel.RefLog;
using ReactiveGit.Gui.Core.ViewModel.Repository;

namespace ReactiveGit.Gui.Core.ViewModel.Factories
{
    /// <summary>
    /// Factory for building a repository.
    /// </summary>
    public interface IRepositoryViewModelFactory
    {
        /// <summary>
        /// Creates a branch view model.
        /// </summary>
        /// <returns>The branch view model.</returns>
        IBranchViewModel CreateBranchViewModel();

        /// <summary>
        /// Creates a commit history view model.
        /// </summary>
        /// <param name="repositoryDetails">The details about the repository.</param>
        /// <returns>The commit history view model.</returns>
        ICommitHistoryViewModel CreateCommitHistoryViewModel(IRepositoryDetails repositoryDetails);

        /// <summary>
        /// Creates a ref log view model.
        /// </summary>
        /// <returns>The ref log view model.</returns>
        IRefLogViewModel CreateRefLogViewModel();

        /// <summary>
        /// Creates the main document.
        /// </summary>
        /// <param name="repositoryDetails">The details about the repository.</param>
        /// <returns>The repository document.</returns>
        IRepositoryDocumentViewModel CreateRepositoryViewModel(IRepositoryDetails repositoryDetails);
    }
}