namespace ReactiveGit.Gui.Base.ViewModel.Branches
{
    using System;
    using System.Collections.Generic;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Windows.Input;

    using ReactiveGit.Gui.Base.Model;
    using ReactiveGit.Model;

    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;

    /// <summary>
    /// View model dealing with branch related issues.
    /// </summary>
    public class BranchViewModel : ReactiveObject, IBranchViewModel
    {
        private readonly ObservableAsPropertyHelper<GitBranch> currentBranch;

        private readonly ReactiveCommand<GitBranch, Unit> checkoutBranch;
      
        private readonly ReactiveCommand<Unit, GitBranch> getBranches;

        private readonly IRepositoryDetails repositoryDetails;

        private readonly ReactiveCommand<Unit, GitBranch> refresh;

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchViewModel"/> class.
        /// </summary>
        /// <param name="repositoryDetails">The details about the repository.</param>
        public BranchViewModel(IRepositoryDetails repositoryDetails)
        {
            if (repositoryDetails == null)
            {
                throw new ArgumentNullException(nameof(repositoryDetails));
            }

            this.repositoryDetails = repositoryDetails;

            this.getBranches = ReactiveCommand.CreateFromObservable(this.GetBranchesImpl);

            this.currentBranch = this.repositoryDetails.BranchManager.CurrentBranch.ToProperty(this, x => x.CurrentBranch, out this.currentBranch);

            var isValidBranch = this.WhenAnyValue(x => x.CurrentBranch).Select(x => x != null);
            this.checkoutBranch = ReactiveCommand.CreateFromObservable<GitBranch, Unit>(x => repositoryDetails.BranchManager.CheckoutBranch(x), isValidBranch);
        }

        /// <summary>
        /// Gets the current branch.
        /// </summary>
        public GitBranch CurrentBranch => this.currentBranch.Value;

        /// <inheritdoc />
        [Reactive]
        public IEnumerable<GitBranch> Branches { get; set; }

        public ICommand Refresh => this.refresh;

        /// <inheritdoc />
        public ICommand CheckoutBranch => this.checkoutBranch;

        private IObservable<GitBranch> GetBranchesImpl()
        {
            this.Branches = this.getBranches.CreateCollection();
            return this.repositoryDetails.BranchManager.GetLocalBranches();
        }
    }
}
