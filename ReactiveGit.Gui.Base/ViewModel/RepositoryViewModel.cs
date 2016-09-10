namespace ReactiveGit.Gui.Base.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Windows.Input;

    using ReactiveGit.ExtensionMethods;
    using ReactiveGit.Gui.Base.ExtensionMethods;
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

        private readonly ObservableAsPropertyHelper<GitBranch> currentBranch;

        private readonly ReactiveCommand<Unit, GitBranch> getCurrentBranch;

        private readonly ReactiveCommand<Unit, GitCommit> getCommitsForBranch;

        private readonly ReactiveCommand<Unit, GitBranch> getBranches;

        private readonly ReactiveCommand<Unit, Unit> changeBranch;

        private readonly ReactiveCommand<Unit, Unit> refresh;

        private IGitProcessManager gitProcessManager;

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

            this.getBranches = ReactiveCommand.CreateFromObservable(() => this.branchManager.GetLocalAndRemoteBranches());
            this.Branches = this.getBranches.CreateCollection();

            IObservable<bool> isCurrentBranchObservable = this.WhenAnyValue(x => x.CurrentBranch).Select(x => x != null);
            this.getCommitsForBranch = ReactiveCommand.CreateFromObservable(() => this.branchManager.GetCommitsForBranch(this.CurrentBranch, 0, 0, GitLogOptions.IncludeMerges), isCurrentBranchObservable);
            this.CommitHistory = this.getCommitsForBranch.CreateCollection();

            this.changeBranch = ReactiveCommand.CreateFromObservable(() => this.branchManager.CheckoutBranch(this.SelectedBranch));

            this.refresh = ReactiveCommand.CreateFromObservable(() => this.getCurrentBranch.Execute().WhenDone().Concat(this.getCommitsForBranch.Execute().WhenDone()));

            this.Refresh.InvokeCommand();
        }

        /// <inheritdoc />
        public string FriendlyName { get; }

        /// <inheritdoc />
        public string RepositoryPath { get; }

        /// <inheritdoc />
        public IReadOnlyCollection<GitCommit> CommitHistory { get; }

        /// <summary>
        /// Gets the current checked out branch.
        /// </summary>
        public GitBranch CurrentBranch => this.currentBranch.Value;

        /// <inheritdoc />
        public ICommand ChangeBranch => this.changeBranch;

        /// <inheritdoc />
        public IReadOnlyList<GitBranch> Branches { get; }

        /// <summary>
        /// Gets a command that will refresh the display.
        /// </summary>
        public ICommand Refresh => this.refresh;

        /// <summary>
        /// Gets or sets the selected branch.
        /// </summary>
        [Reactive]
        public GitBranch SelectedBranch { get; set; }
    }
}
