namespace ReactiveGit.Gui.Base.Model
{
    using System;
    using System.IO;

    using ReactiveGit.Managers;

    /// <summary>
    /// Details about a repository.
    /// </summary>
    public class RepositoryDetails : IRepositoryDetails
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

            var gitProcessManager = new GitProcessManager(repositoryPath, null);
            this.BranchManager = new BranchManager(gitProcessManager);
            this.RebaseManager = new RebaseManager(gitProcessManager, this.BranchManager);
            this.RefLogManager = new RefLogManager(gitProcessManager);
            this.FriendlyName = Path.GetFileName(repositoryPath);
        }

        /// <inheritdoc />
        public IBranchManager BranchManager { get; }

        /// <inheritdoc />
        public IRebaseManager RebaseManager { get; }

        /// <inheritdoc />
        public IRefLogManager RefLogManager { get; }

        /// <inheritdoc />
        public string FriendlyName { get; }

        /// <inheritdoc />
        public string RepositoryPath { get; }
    }
}