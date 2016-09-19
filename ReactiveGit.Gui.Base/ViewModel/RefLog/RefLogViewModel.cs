namespace ReactiveGit.Gui.Base.ViewModel.RefLog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using ReactiveGit.ExtensionMethods;
    using ReactiveGit.Gui.Base.ExtensionMethods;
    using ReactiveGit.Gui.Base.Model;
    using ReactiveGit.Model;

    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;

    /// <summary>
    /// Implementation of the ref log view model.
    /// </summary>
    public class RefLogViewModel : ReactiveObject, IRefLogViewModel
    {
        private readonly IRepositoryDetails repositoryDetails;

        private readonly ReactiveCommand<Unit, GitRefLog> refresh;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefLogViewModel"/> class.
        /// </summary>
        /// <param name="repositoryDetails">The details about the repository.</param>
        /// <exception cref="ArgumentNullException">Must have valid repository details.</exception>
        public RefLogViewModel(IRepositoryDetails repositoryDetails)
        {
            if (repositoryDetails == null)
            {
                throw new ArgumentNullException(nameof(repositoryDetails));
            }

            this.repositoryDetails = repositoryDetails;

            IObservable<bool> isCurrentBranchObservable = this.WhenAnyValue(x => x.CurrentBranch).Select(x => x != null);

            this.refresh = ReactiveCommand.CreateFromObservable(this.RefreshImpl, isCurrentBranchObservable);

            this.WhenAnyValue(x => x.CurrentBranch).Subscribe(_ => this.Refresh.InvokeCommand());
        }

        /// <inheritdoc />
        [Reactive]
        public IEnumerable<GitRefLog> RefLog { get; set; }

        /// <inheritdoc />
        [Reactive]
        public GitBranch CurrentBranch { get; set; }

        /// <inheritdoc />
        public ICommand ResetHardRefLog { get; }

        /// <inheritdoc />
        public ICommand ResetSoftRefLog { get; }

        /// <inheritdoc />
        public ICommand Refresh => this.refresh;

        private IObservable<GitRefLog> RefreshImpl()
        {
            this.RefLog = this.refresh.CreateCollection();

            return this.repositoryDetails.RefLogManager.GetRefLog(this.CurrentBranch);
        }
    }
}
