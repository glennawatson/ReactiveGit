namespace ReactiveGit.Gui.Core.ViewModel.RefLog
{
    using System;
    using System.Collections.Generic;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Windows.Input;

    using ReactiveGit.Core.ExtensionMethods;
    using ReactiveGit.Core.Model;
    using ReactiveGit.Gui.Core.ExtensionMethods;
    using ReactiveGit.Gui.Core.ViewModel.GitObject;

    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;

    /// <summary>
    /// Implementation of the ref log view model.
    /// </summary>
    public class RefLogViewModel : GitObjectViewModelBase, IRefLogViewModel
    {
        private readonly ReactiveCommand<Unit, GitRefLog> refresh;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefLogViewModel"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">Must have valid repository details.</exception>
        public RefLogViewModel()
        {
            IObservable<bool> isCurrentBranchObservable =
                this.WhenAnyValue(x => x.RepositoryDetails.SelectedBranch).Select(details => details != null);

            this.refresh = ReactiveCommand.CreateFromObservable(this.RefreshImpl, isCurrentBranchObservable);

            this.WhenAnyValue(x => x.RepositoryDetails.SelectedBranch).Where(x => x != null).Subscribe(_ => this.Refresh.InvokeCommand());
        }

        /// <inheritdoc />
        public override string FriendlyName => "Undo Stack";

        /// <inheritdoc />
        [Reactive]
        public IEnumerable<GitRefLog> RefLog { get; set; }

        /// <inheritdoc />
        public override ICommand Refresh => this.refresh;

        private IObservable<GitRefLog> RefreshImpl()
        {
            this.RefLog = this.refresh.CreateCollection();

            return this.RepositoryDetails.RefLogManager.GetRefLog(this.RepositoryDetails.SelectedBranch);
        }
    }
}