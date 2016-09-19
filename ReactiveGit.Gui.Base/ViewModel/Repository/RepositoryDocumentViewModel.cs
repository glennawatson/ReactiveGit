namespace ReactiveGit.Gui.Base.ViewModel.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;

    using ReactiveGit.Gui.Base.Model;
    using ReactiveGit.Gui.Base.Model.Branches;
    using ReactiveGit.Gui.Base.ViewModel.Branches;
    using ReactiveGit.Gui.Base.ViewModel.CommitHistory;
    using ReactiveGit.Gui.Base.ViewModel.Factories;
    using ReactiveGit.Gui.Base.ViewModel.RefLog;
    using ReactiveGit.Model;

    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;

    /// <summary>
    /// View model associated with a repository document.
    /// </summary>
    public class RepositoryDocumentViewModel : ReactiveObject, IRepositoryDocumentViewModel
    {
        private readonly IRepositoryDetails repositoryDetails;

        private readonly IList<IChildRepositoryDocumentViewModel> childRepositoryViewModels;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryDocumentViewModel"/> class.
        /// </summary>
        /// <param name="factory">A factory for creating the children.</param>
        /// <param name="repositoryDetails">The details about the repositories.</param>
        public RepositoryDocumentViewModel(IRepositoryViewModelFactory factory, IRepositoryDetails repositoryDetails)
        {
            this.repositoryDetails = repositoryDetails;
            this.CommitHistoryViewModel = factory.CreateCommitHistoryViewModel(this.repositoryDetails);
            this.BranchViewModel = factory.CreateBranchViewModel(this.repositoryDetails);
            this.RefLogViewModel = factory.CreateRefLogViewModel(this.repositoryDetails);

            this.childRepositoryViewModels = new List<IChildRepositoryDocumentViewModel>
                                                 {
                                                     this.BranchViewModel,
                                                     this.RefLogViewModel,
                                                     this.CommitHistoryViewModel
                                                 };

            this.WhenAnyValue(x => x.SelectedBranchNode)
                .Select(x => x as BranchLeaf)
                .Where(x => x != null)
                .Select(x => x.Branch)
                .Subscribe(this.UpdateCurrentBranch);
        }

        /// <inheritdoc />
        public ICommitHistoryViewModel CommitHistoryViewModel { get; }

        /// <inheritdoc />
        public IBranchViewModel BranchViewModel { get; }

        /// <inheritdoc />
        public IRefLogViewModel RefLogViewModel { get; }

        /// <inheritdoc />
        public string FriendlyName => this.repositoryDetails.FriendlyName;

        /// <inheritdoc />
        public string RepositoryPath => this.repositoryDetails.RepositoryPath;

        /// <inheritdoc />
        [Reactive]
        public BranchNode SelectedBranchNode { get; set; }

        private void UpdateCurrentBranch(GitBranch x)
        {
            foreach (IChildRepositoryDocumentViewModel childDocument in this.childRepositoryViewModels)
            {
                childDocument.CurrentBranch = x;
            }
        }
    }
}