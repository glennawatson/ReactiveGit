// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveGit.Gui.Core.Model;

namespace ReactiveGit.Gui.Core.ViewModel
{
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