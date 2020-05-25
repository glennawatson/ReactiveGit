// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using ReactiveGit.Gui.Core.Model;
using ReactiveGit.Library.Core.Managers;
using ReactiveGit.Library.Core.Model;
using ReactiveGit.Library.RunProcess.Managers;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ReactiveGit.Gui.WPF.Factories
{
    /// <summary>
    /// Details about a repository.
    /// </summary>
    public class RepositoryDetails : ReactiveObject, IRepositoryDetails
    {
        /// <summary>Initializes a new instance of the <see cref="RepositoryDetails"/> class.</summary>
        /// <param name="repositoryPath">The path to the repository.</param>
        public RepositoryDetails(string repositoryPath)
        {
            if (Directory.Exists(repositoryPath) == false)
            {
                throw new ArgumentException(nameof(repositoryPath));
            }

            RepositoryPath = repositoryPath;

            var gitProcessManager = new GitProcessManager(repositoryPath);
            BranchManager = new BranchManager(gitProcessManager);
            RebaseManager = new RebaseManager(gitProcessManager, BranchManager);
            RefLogManager = new RefLogManager(gitProcessManager);
            GitObjectManager = new GitObjectManager(gitProcessManager);
            TagManager = new TagManager(gitProcessManager);
            FriendlyName = Path.GetFileName(repositoryPath);
            RepositoryManager = gitProcessManager;
        }

        /// <inheritdoc />
        public IBranchManager BranchManager { get; }

        /// <inheritdoc />
        public string FriendlyName { get; }

        /// <inheritdoc />
        public IGitObjectManager GitObjectManager { get; }

        /// <inheritdoc />
        public IRebaseManager RebaseManager { get; }

        /// <inheritdoc />
        public IRefLogManager RefLogManager { get; }

        /// <inheritdoc />
        public IGitRepositoryManager RepositoryManager { get; }

        /// <inheritdoc />
        public ITagManager TagManager { get; }

        /// <inheritdoc />
        public string RepositoryPath { get; }

        /// <inheritdoc />
        [Reactive]
        public GitBranch SelectedBranch { get; set; }
    }
}