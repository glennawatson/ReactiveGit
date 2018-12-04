// <copyright file="IOutputViewModel.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.Core.ViewModel.Output
{
    using System.Windows.Input;

    /// <summary>
    /// Represents the window for output from GIT.
    /// </summary>
    public interface IOutputViewModel : ISupportViewModel
    {
        /// <summary>
        /// Gets a command which will clear the output.
        /// </summary>
        ICommand Clear { get; }

        /// <summary>
        /// Gets the output from GIT.
        /// </summary>
        string Output { get; }
    }
}