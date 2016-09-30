namespace ReactiveGit.Gui.Core.ViewModel.GitObject
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Windows.Input;

    using ReactiveGit.Core.Model;
    using ReactiveGit.Gui.Core.Model;
    using ReactiveGit.Gui.Core.ViewModel.Content;

    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;

    /// <summary>
    /// Base class for classes handling git objects.
    /// </summary>
    public abstract class GitObjectViewModelBase : ContentViewModelBase, IGitObjectViewModel
    {
        private readonly ReactiveCommand<IGitIdObject, Unit> resetHard;

        private readonly ReactiveCommand<IGitIdObject, Unit> resetMixed;

        private readonly ReactiveCommand<IGitIdObject, Unit> resetSoft;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitObjectViewModelBase" /> class.
        /// </summary>
        protected GitObjectViewModelBase()
        {
            IObservable<bool> canReset =
                this.WhenAnyValue(x => x.RepositoryDetails, x => x.RepositoryDetails.SelectedBranch).Select(
                    x => (x.Item1 != null) && (x.Item2 != null));
            this.resetHard =
                ReactiveCommand.CreateFromObservable<IGitIdObject, Unit>(
                    x => this.RepositoryDetails.GitObjectManager.Reset(x, ResetMode.Hard),
                    canReset);
            this.resetSoft =
                ReactiveCommand.CreateFromObservable<IGitIdObject, Unit>(
                    x => this.RepositoryDetails.GitObjectManager.Reset(x, ResetMode.Soft),
                    canReset);
            this.resetMixed =
                ReactiveCommand.CreateFromObservable<IGitIdObject, Unit>(
                    x => this.RepositoryDetails.GitObjectManager.Reset(x, ResetMode.Mixed),
                    canReset);
        }

        /// <inheritdoc />
        public abstract ICommand Refresh { get; }

        /// <summary>
        /// Gets or sets the repository details.
        /// </summary>
        [Reactive]
        public IRepositoryDetails RepositoryDetails { get; set; }

        /// <inheritdoc />
        public ICommand ResetHard => this.resetHard;

        /// <inheritdoc />
        public ICommand ResetMixed => this.resetMixed;

        /// <inheritdoc />
        public ICommand ResetSoft => this.resetSoft;

        /// <inheritdoc />
        [Reactive]
        public IGitIdObject SelectedGitObject { get; set; }
    }
}