// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Input;

using ReactiveUI;

namespace ReactiveGit.Gui.WPF.ViewModel
{
    /// <summary>
    /// A view model relating to saving window related information.
    /// </summary>
    public interface IWindowLayoutViewModel : IReactiveObject
    {
        /// <summary>
        /// Gets or sets the window that the.
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