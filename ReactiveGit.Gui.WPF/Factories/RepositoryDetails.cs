namespace ReactiveGit.Gui.WPF.Factories
{
    using System;
    using System.IO;

    using ReactiveGit.Core.Managers;
    using ReactiveGit.Core.Model;
    using ReactiveGit.Gui.Core.Model;
    using ReactiveGit.Process.Managers;

    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;

    /// <summary>
    /// Details about a repository.
    /// </summary>
    public class RepositoryDetails : ReactiveObject, IRepositoryDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryDetails"/> class.
        /// </summary>
        /// <param name="repositoryPath">The path to the repository.</param>
        public RepositoryDetails(string repositoryPath)
        {
            if (Directory.Exists(repositoryPath) == false)
            {
                throw new ArgumentException(nameof(repositoryPath));
            }

            this.RepositoryPath = repositoryPath;

            var gitProcessManager = new GitProcessManager(repositoryPath);
            this.BranchManager = new BranchManager(gitProcessManager);
            this.RebaseManager = new RebaseManager(gitProcessManager, this.BranchManager);
            this.RefLogManager = new RefLogManager(gitProcessManager);
            this.GitObjectManager = new GitObjectManager(gitProcessManager);
            this.FriendlyName = Path.GetFileName(repositoryPath);
            this.RepositoryManager = gitProcessManager;
        }

        /// <inheritdoc />
        public IBranchManager BranchManager { get; }

        /// <inheritdoc />
        public IRebaseManager RebaseManager { get; }

        /// <inheritdoc />
        public IRefLogManager RefLogManager { get; }

        /// <inheritdoc />
        public IGitRepositoryManager RepositoryManager { get; }

        /// <inheritdoc />
        public IGitObjectManager GitObjectManager { get; }

        /// <inheritdoc />
        public string FriendlyName { get; }

        /// <inheritdoc />
        public string RepositoryPath { get; }

        /// <inheritdoc />
        [Reactive]
        public GitBranch SelectedBranch { get; set; }
    }
}