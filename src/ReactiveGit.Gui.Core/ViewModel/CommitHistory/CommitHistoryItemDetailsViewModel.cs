// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

using ReactiveUI;

namespace ReactiveGit.Gui.Core.ViewModel.CommitHistory
{
    /// <summary>
    /// Details about a commit.
    /// </summary>
    public class CommitHistoryItemDetailsViewModel : ReactiveObject
    {
        /// <summary>
        /// Gets or sets the date time of the commit.
        /// </summary>
        public DateTime DateTime { get; set; }
    }
}