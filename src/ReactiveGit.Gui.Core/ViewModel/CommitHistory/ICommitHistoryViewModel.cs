// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;

using ReactiveGit.Gui.Core.ViewModel.GitObject;

namespace ReactiveGit.Gui.Core.ViewModel.CommitHistory
{
    /// <summary>
    /// A view model for a GIT repository.
    /// </summary>
    public interface ICommitHistoryViewModel : IGitObjectViewModel
    {
        /// <summary>
        /// Gets a collection of commit history.
        /// </summary>
        IEnumerable<CommitHistoryItemViewModel> CommitHistory { get; }
    }
}