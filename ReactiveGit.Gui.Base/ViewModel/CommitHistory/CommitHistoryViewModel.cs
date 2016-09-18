namespace ReactiveGit.Gui.Base.ViewModel.CommitHistory
{
    using System;
    using System.Collections.Generic;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Windows.Input;

    using ReactiveGit.Gui.Base.ExtensionMethods;
    using ReactiveGit.Gui.Base.Model;
    using ReactiveGit.Gui.Base.Properties;
    using ReactiveGit.Model;

    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;

    /// <summary>
    /// A view model related to a repository.
    /// </summary>
    public class CommitHistoryViewModel : ReactiveObject, ICommitHistoryViewModel
    {
        private readonly ReactiveCommand<Unit, GitCommit> refresh;

        private readonly IRepositoryDetails repositoryDetails;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommitHistoryViewModel"/> class.
        /// </summary>
        /// <param name="repositoryDetails">The details of the repository.</param>
        /// <exception cref="ArgumentException">If the repository does not exist.</exception>
        public CommitHistoryViewModel(IRepositoryDetails repositoryDetails)
        {
            this.repositoryDetails = repositoryDetails;
            IObservable<bool> isCurrentBranchObservable = this.WhenAnyValue(x => x.CurrentBranch).Select(x => x != null);

            this.refresh = ReactiveCommand.CreateFromObservable(() => this.GetCommitsImpl(this.CurrentBranch), isCurrentBranchObservable);

            isCurrentBranchObservable.Subscribe(x => this.Refresh.InvokeCommand());

            this.Refresh.InvokeCommand();

            if (Settings.Default.AutomaticRefreshIntervalMinutes > 0)
            {
                Observable.Interval(TimeSpan.FromMinutes(Settings.Default.AutomaticRefreshIntervalMinutes)).InvokeCommand(this.refresh);
            }
        }

        /// <inheritdoc />
        [Reactive]
        public IEnumerable<GitCommit> CommitHistory { get; set; }

        /// <summary>
        /// Gets or sets the current checked out branch.
        /// </summary>
        [Reactive]
        public GitBranch CurrentBranch { get; set; }

        /// <summary>
        /// Gets a command that will refresh the display.
        /// </summary>
        public ICommand Refresh => this.refresh;

        /// <summary>
        /// Gets or sets the selected branch.
        /// </summary>
        [Reactive]
        public GitBranch SelectedBranch { get; set; }

        private IObservable<GitCommit> GetCommitsImpl(GitBranch branch)
        {
            this.CommitHistory = this.refresh.CreateCollection();
            return this.repositoryDetails.BranchManager.GetCommitsForBranch(branch, 0, 0, GitLogOptions.IncludeMerges);
        }
    }
}