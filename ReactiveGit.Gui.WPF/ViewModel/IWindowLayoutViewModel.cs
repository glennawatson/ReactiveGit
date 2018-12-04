// <copyright file="IWindowLayoutViewModel.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.WPF.ViewModel
{
    using System.Windows;
    using System.Windows.Input;

    using ReactiveUI;

    /// <summary>
    /// A view model relating to saving window related information.
    /// </summary>
    public interface IWindowLayoutViewModel : IReactiveObject
    {
        /// <summary>
        /// Gets or sets the window that the 
        /// </summary>
        Window Window { get; set; }

        /// <summary>
        /// Gets a command that will save the window placement data.
        /// </summary>
        ICommand Save { get; }

        /// <summary>
        /// Gets a command that will load the window placement data.
        /// </summary>
        ICommand Load { get; }
    }
}