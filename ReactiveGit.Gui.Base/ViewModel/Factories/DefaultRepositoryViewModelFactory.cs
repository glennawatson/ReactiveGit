namespace ReactiveGit.Gui.Base.ViewModel.Factories
{
    using ReactiveGit.Gui.Base.Model;
    using ReactiveGit.Gui.Base.ViewModel.Branches;
    using ReactiveGit.Gui.Base.ViewModel.CommitHistory;
    using ReactiveGit.Gui.Base.ViewModel.RefLog;
    using ReactiveGit.Gui.Base.ViewModel.Repository;

    /// <summary>
    /// The default factory for generating repository view models.  
    /// </summary>
    public class DefaultRepositoryViewModelFactory : IRepositoryViewModelFactory
    {
        /// <inheritdoc />
        public IRepositoryDocumentViewModel CreateRepositoryViewModel(IRepositoryDetails repositoryDetails)
        {
            return new RepositoryDocumentViewModel(this, repositoryDetails);
        }

        /// <inheritdoc />
        public ICommitHistoryViewModel CreateCommitHistoryViewModel(IRepositoryDetails repositoryDetails)
        {
            return new CommitHistoryViewModel(repositoryDetails);
        }

        /// <inheritdoc />
        public IBranchViewModel CreateBranchViewModel(IRepositoryDetails repositoryDetails)
        {
            return new BranchViewModel(repositoryDetails);
        }

        /// <inheritdoc />
        public IRefLogViewModel CreateRefLogViewModel(IRepositoryDetails repositoryDetails)
        {
            return new RefLogViewModel(repositoryDetails);
        }
    }
}