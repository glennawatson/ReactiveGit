namespace ReactiveGit.Gui.Base.ViewModel
{
    using System;
    using System.Reactive;
    using System.Windows.Input;

    using ReactiveGit.Gui.Base.Model;
    using ReactiveGit.Model;

    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;

    /// <summary>
    /// Base class for classes handling git objects. 
    /// </summary>
    public abstract class GitObjectViewModelBase : ReactiveObject, IGitObjectViewModel
    {
        private readonly ReactiveCommand<IGitIdObject, Unit> resetHard;
        private readonly ReactiveCommand<IGitIdObject, Unit> resetSoft;
        private readonly ReactiveCommand<IGitIdObject, Unit> resetMixed;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitObjectViewModelBase"/> class.
        /// </summary>
        /// <param name="repositoryDetails">The details about the repository.</param>
        protected GitObjectViewModelBase(IRepositoryDetails repositoryDetails)
        {
            if (repositoryDetails == null)
            {
                throw new ArgumentNullException(nameof(repositoryDetails));
            }

            this.RepositoryDetails = repositoryDetails;

            this.resetHard = ReactiveCommand.CreateFromObservable<IGitIdObject, Unit>(x => this.RepositoryDetails.GitObjectManager.Reset(x, ResetMode.Hard));
            this.resetSoft = ReactiveCommand.CreateFromObservable<IGitIdObject, Unit>(x => this.RepositoryDetails.GitObjectManager.Reset(x, ResetMode.Soft));
            this.resetMixed = ReactiveCommand.CreateFromObservable<IGitIdObject, Unit>(x => this.RepositoryDetails.GitObjectManager.Reset(x, ResetMode.Mixed));
        }

        /// <inheritdoc />
        public abstract ICommand Refresh { get; }

        /// <inheritdoc />
        [Reactive]
        public GitBranch CurrentBranch { get; set; }

        /// <inheritdoc />
        public ICommand ResetHard => this.resetHard;

        /// <inheritdoc />
        public ICommand ResetSoft => this.resetSoft;

        /// <inheritdoc />
        public ICommand ResetMixed => this.resetMixed;

        /// <inheritdoc />
        [Reactive]
        public IGitIdObject SelectedGitObject { get; set; }

        /// <summary>
        /// Gets or sets the repository details.
        /// </summary>
        [Reactive]
        protected IRepositoryDetails RepositoryDetails { get; set; }
    }
}
