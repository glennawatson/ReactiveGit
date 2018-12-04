// <copyright file="ILayoutViewModel.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.WPF.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using ReactiveUI;

    using Xceed.Wpf.AvalonDock;

    /// <summary>
    /// View model for handling docking layout. 
    /// </summary>
    public interface ILayoutViewModel : IReactiveObject
    {
        /// <summary>
        /// Gets or sets the docking manager.
        /// </summary>
        DockingManager DockingManager { get; set; }

        /// <summary>
        /// Gets or sets the selected layout file.
        /// </summary>
        string SelectedLayout { get; set; }

        /// <summary>
        /// Gets the name of the saved layouts.
        /// </summary>
        IEnumerable<string> SavedLayoutNames { get; }

            /// <summary>
        /// Gets a command that will open the layout.
        /// </summary>
        ICommand OpenLayout { get; }

        /// <summary>
        /// Gets a command that will save the current layout. 
        /// </summary>
        ICommand SaveLayout { get; }

        /// <summary>
        /// Gets a command that will reset the layout.
        /// </summary>
        ICommand ResetLayout { get; }

        /// <summary>
        /// Gets a command which can be activated on startup.
        /// </summary>
        ICommand Start { get; }

        /// <summary>
        /// Gets a command which can be activated on shutdown.
        /// </summary>
        ICommand Shutdown { get; }
    }
}
