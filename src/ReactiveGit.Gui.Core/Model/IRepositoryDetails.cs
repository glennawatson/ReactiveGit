// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;
using ReactiveGit.Library.Core.Managers;
using ReactiveGit.Library.Core.Model;

namespace ReactiveGit.Gui.Core.Model
{
    /// <summary>
    /// Details about the repository.
    /// </summary>
    public interface IRepositoryDetails : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the branch manager.
        /// </summary>
        IBranchManager BranchManager { get; }

        /// <summary>
        /// Gets the friendly name of the repository.
        /// </summary>
        string FriendlyName { get; }

        /// <summary>
        /// Gets the git object manager.
        /// </summary>
        IGitObjectManager GitObjectManager { get; }

        /// <summary>
        /// Gets the rebase manager.
        /// </summary>
        IRebaseManager RebaseManager { get; }

        /// <summary>
        /// Gets the ref log manager.
        /// </summary>
        IRefLogManager RefLogManager { get; }

        /// <summary>
        /// Gets the repository manager.
        /// </summary>
        IGitRepositoryManager RepositoryManager { get; }

        /// <summary>
        /// Gets the tag manager.
        /// </summary>
        ITagManager TagManager { get; }

        /// <summary>
        /// Gets the path to the repository.
        /// </summary>
        string RepositoryPath { get; }

        /// <summary>
        /// Gets or sets the selected branch.
        /// </summary>
        GitBranch SelectedBranch { get; set; }
    }
}