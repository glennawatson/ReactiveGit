// <copyright file="IRepositoryFactory.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.Core.Model.Factories
{
    using ReactiveGit.Core.Managers;

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