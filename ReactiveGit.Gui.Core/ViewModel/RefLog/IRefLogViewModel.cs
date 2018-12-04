// <copyright file="IRefLogViewModel.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.Core.ViewModel.RefLog
{
    using System.Collections.Generic;

    using ReactiveGit.Core.Model;
    using ReactiveGit.Gui.Core.ViewModel.GitObject;

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