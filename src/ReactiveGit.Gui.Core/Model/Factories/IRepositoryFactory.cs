// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveGit.Library.Core.Managers;

namespace ReactiveGit.Gui.Core.Model.Factories
{
    /// <summary>
    /// Factory for generating classes relating to the repository.
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Creates a new repository creator.
        /// </summary>
        /// <returns>The repository creator.</returns>
        IRepositoryCreator CreateRepositoryCreator();

        /// <summary>
        /// Creates a new repository for the specified repository directory.
        /// </summary>
        /// <param name="repositoryDirectory">The path to the repository.</param>
        /// <returns>The repository details.</returns>
        IRepositoryDetails CreateRepositoryDetails(string repositoryDirectory);
    }
}