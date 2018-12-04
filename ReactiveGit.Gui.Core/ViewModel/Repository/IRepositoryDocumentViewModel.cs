// <copyright file="IRepositoryDocumentViewModel.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.Core.ViewModel.Repository
{
    using ReactiveGit.Gui.Core.Model;
    using ReactiveGit.Gui.Core.ViewModel.CommitHistory;

    /// <summary>
    /// A document for the repository.
    /// </summary>
    public interface IRepositoryDocumentViewModel
    {
        /// <summary>
        /// Gets the view model associated with the commit history of the repository.
        /// </summary>
        ICommitHistoryViewModel CommitHistoryViewModel { get; }

        /// <summary>
        /// Gets a user friendly name of the repository.
        /// </summary>
        string FriendlyName { get; }

        /// <summary>
        /// Gets the details about the repository.
        /// </summary>
        IRepositoryDetails RepositoryDetails { get; }

        /// <summary>
        /// Gets the path to the repository.
        /// </summary>
        string RepositoryPath { get; }
    }
}