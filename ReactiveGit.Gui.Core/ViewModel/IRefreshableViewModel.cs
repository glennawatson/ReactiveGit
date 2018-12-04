// <copyright file="IRefreshableViewModel.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.Core.ViewModel
{
    using System.Windows.Input;

    /// <summary>
    /// A view model that can be refreshed.
    /// </summary>
    public interface IRefreshableViewModel : IViewModel
    {
        /// <summary>
        /// Gets a command which will refresh the child repository document.
        /// </summary>
        ICommand Refresh { get; }
    }
}