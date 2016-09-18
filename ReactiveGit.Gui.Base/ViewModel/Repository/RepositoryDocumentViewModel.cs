namespace ReactiveGit.Gui.Base.ViewModel.Repository
{
    using ReactiveGit.Gui.Base.Model;
    using ReactiveGit.Gui.Base.ViewModel.Branches;
    using ReactiveGit.Gui.Base.ViewModel.CommitHistory;
    using ReactiveGit.Gui.Base.ViewModel.Factories;

    /// <summary>
    /// View model associated with a repository document.
    /// </summary>
    public class RepositoryDocumentViewModel : IRepositoryDocumentViewModel
    {
        private readonly IRepositoryDetails repositoryDetails;

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
        }

        /// <inheritdoc />
        public ICommitHistoryViewModel CommitHistoryViewModel { get; }

        /// <inheritdoc />
        public IBranchViewModel BranchViewModel { get; }

        /// <inheritdoc />
        public string FriendlyName => this.repositoryDetails.FriendlyName;

        /// <inheritdoc />
        public string RepositoryPath => this.repositoryDetails.RepositoryPath;
    }
}