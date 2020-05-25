// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using ReactiveGit.Gui.Core.ViewModel.GitObject;
using ReactiveGit.Library.Core.Model;

namespace ReactiveGit.Gui.Core.ViewModel.RefLog
{
    /// <summary>
    /// View model for handling ref log.
    /// </summary>
    public interface IRefLogViewModel : IGitObjectViewModel, ISupportViewModel
    {
        /// <summary>
        /// Gets a collection of ref log entries for the current branch.
        /// </summary>
        IEnumerable<GitRefLog> RefLog { get; }
    }
}