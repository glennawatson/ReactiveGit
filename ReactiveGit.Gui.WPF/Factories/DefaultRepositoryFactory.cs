// <copyright file="DefaultRepositoryFactory.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.WPF.Factories
{
    using ReactiveGit.Core.Managers;
    using ReactiveGit.Gui.Core.Model;
    using ReactiveGit.Gui.Core.Model.Factories;
    using ReactiveGit.Process.Managers;

    /// <summary>
    /// The default repository details factory.
    /// </summary>
    public class DefaultRepositoryFactory : IRepositoryFactory
    {
        /// <inheritdoc />
        public IRepositoryCreator CreateRepositoryCreator()
        {
            return new RepositoryCreator(x => new GitProcessManager(x));
        }

        /// <inheritdoc />
        public IRepositoryDetails CreateRepositoryDetails(string repositoryDirectory)
        {
            return new RepositoryDetails(repositoryDirectory);
        }
    }
}