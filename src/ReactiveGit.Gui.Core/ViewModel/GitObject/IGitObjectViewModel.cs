// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Windows.Input;

using ReactiveGit.Library.Core.Model;

using ReactiveUI;

namespace ReactiveGit.Gui.Core.ViewModel.GitObject
{
    /// <summary>
    /// View model dealing with GIT objects.
    /// </summary>
    public interface IGitObjectViewModel : IRefreshableViewModel
    {
        /// <summary>
        /// Gets a collection of actions that can be performed on the git object.
        /// </summary>
        IReadOnlyList<IGitObjectAction> Actions { get; }

            /// <summary>
        /// Gets or sets the selected GIT object.
        /// </summary>
        IGitIdObject SelectedGitObject { get; set; }
    }
}