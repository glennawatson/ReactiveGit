// <copyright file="IViewModel.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.Core.ViewModel
{
    using ReactiveGit.Gui.Core.Model;

    /// <summary>
    /// A view model in the reactive git program.
    /// </summary>
    public interface IViewModel
    {
        /// <summary>
        /// Gets or sets the repository details.
        /// </summary>
        IRepositoryDetails RepositoryDetails { get; set; }
    }
}