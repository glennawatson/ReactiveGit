// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveGit.Gui.Core.Model;
using ReactiveGit.Gui.Core.ViewModel.CommitHistory;

namespace ReactiveGit.Gui.Core.ViewModel.Repository
{
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