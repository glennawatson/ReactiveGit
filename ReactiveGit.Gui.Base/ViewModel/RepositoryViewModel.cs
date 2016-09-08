namespace ReactiveGit.Gui.Base.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using ReactiveGit.Managers;
    using ReactiveGit.Model;

    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;

    /// <summary>
    /// A view model related to a repository.
    /// </summary>
    public class RepositoryViewModel : ReactiveObject, IRepositoryViewModel
    {
        private readonly IBranchManager branchManager;

        private IGitProcessManager gitProcessManager;
        private ObservableAsPropertyHelper<GitBranch> currentBranch;

        private ReactiveCommand<Unit, GitBranch> getCurrentBranch;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryViewModel"/> class.
        /// </summary>
        /// <param name="repositoryPath">The path to the repository.</param>
        /// <exception cref="ArgumentException">If the repository does not exist.</exception>
        public RepositoryViewModel(string repositoryPath)
        {
            if (Directory.Exists(repositoryPath) == false)
            {
                throw new ArgumentException(nameof(repositoryPath));
            }

            this.gitProcessManager = new GitProcessManager(repositoryPath, null);
            this.branchManager = new BranchManager(repositoryPath, null);

            this.FriendlyName = Path.GetFileName(repositoryPath);
            this.RepositoryPath = repositoryPath;

            this.getCurrentBranch = ReactiveCommand.CreateFromObservable(() => this.branchManager.GetCurrentCheckedOutBranch());
            this.currentBranch = this.getCurrentBranch.ToProperty(this, x => x.CurrentBranch, out this.currentBranch);
        }

        /// <inheritdoc />
        public string FriendlyName { get; }

        /// <inheritdoc />
        public string RepositoryPath { get; }

        /// <inheritdoc />
        public IReadOnlyCollection<GitCommit> CommitHistory { get; }

        public GitBranch CurrentBranch => this.currentBranch.Value;

        /// <inheritdoc />
        public ICommand ChangeBranch { get; }

        /// <inheritdoc />
        public IReadOnlyList<GitBranch> Branches { get; }

        public ICommand Refresh { get; }

    }
}
