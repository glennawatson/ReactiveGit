// <copyright file="IGitObjectViewModel.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.Core.ViewModel.GitObject
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using ReactiveGit.Core.Model;

    using ReactiveUI;

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