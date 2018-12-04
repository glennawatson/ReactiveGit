// <copyright file="CommitHistoryItemDetailsViewModel.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.Core.ViewModel.CommitHistory
{
    using System;

    using ReactiveUI;

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