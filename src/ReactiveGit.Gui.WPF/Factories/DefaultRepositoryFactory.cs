// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveGit.Gui.Core.Model;
using ReactiveGit.Gui.Core.Model.Factories;
using ReactiveGit.Library.Core.Managers;
using ReactiveGit.Library.RunProcess.Managers;

namespace ReactiveGit.Gui.WPF.Factories
{
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