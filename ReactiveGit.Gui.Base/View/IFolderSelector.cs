// <copyright file="IFolderSelector.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ReactiveGit.Gui.Base.View
{
    using System;

    /// <summary>
    /// A interface to a folder selector.
    /// </summary>
    public interface IFolderSelector
    {
        /// <summary>
        /// Prompts the user for a folder.
        /// </summary>
        /// <returns>A observable with a string to the folder.</returns>
        IObservable<string> Prompt();
    }
}
