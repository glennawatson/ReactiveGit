namespace ReactiveGit.Gui.Base.ViewModel.Factories
{
    using ReactiveGit.Gui.Base.Model;
    using ReactiveGit.Gui.Base.ViewModel.Branches;
    using ReactiveGit.Gui.Base.ViewModel.CommitHistory;
    using ReactiveGit.Gui.Base.ViewModel.RefLog;
    using ReactiveGit.Gui.Base.ViewModel.Repository;

    /// <summary>
    /// Factory for building a repository.
    /// </summary>
    public interface IRepositoryViewModelFactory
    {
        /// <summary>
        /// Creates the main document.
        /// </summary>
        /// <param name="repositoryDetails">The details about the repository.</param>
        /// <returns>The repository document.</returns>
        IRepositoryDocumentViewModel CreateRepositoryViewModel(IRepositoryDetails repositoryDetails);

        /// <summary>
        /// Creates a commit history view model.
        /// </summary>
        /// <param name="repositoryDetails">The details about the repository.</param>
        /// <returns>The commit history view model.</returns>
        ICommitHistoryViewModel CreateCommitHistoryViewModel(IRepositoryDetails repositoryDetails);

        /// <summary>
        /// Creates a branch view model.
        /// </summary>
        /// <param name="repositoryDetails">The details about the repository.</param>
        /// <returns>The branch view model.</returns>
        IBranchViewModel CreateBranchViewModel(IRepositoryDetails repositoryDetails);

        /// <summary>
        /// Creates a ref log view model.
        /// </summary>
        /// <param name="repositoryDetails">The details about the repository.</param>
        /// <returns>The ref log view model.</returns>
        IRefLogViewModel CreateRefLogViewModel(IRepositoryDetails repositoryDetails);
    }
}