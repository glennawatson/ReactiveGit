namespace ReactiveGit.Gui.Core.ViewModel.Repository
{
    using ReactiveGit.Gui.Core.Model;
    using ReactiveGit.Gui.Core.Model.Branches;
    using ReactiveGit.Gui.Core.ViewModel.CommitHistory;
    using ReactiveGit.Gui.Core.ViewModel.Factories;

    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;

    /// <summary>
    /// View model associated with a repository document.
    /// </summary>
    public class RepositoryDocumentViewModel : ReactiveObject, IRepositoryDocumentViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryDocumentViewModel"/> class.
        /// </summary>
        /// <param name="factory">A factory for creating the children.</param>
        /// <param name="repositoryDetails">The details about the repositories.</param>
        public RepositoryDocumentViewModel(IRepositoryViewModelFactory factory, IRepositoryDetails repositoryDetails)
        {
            this.RepositoryDetails = repositoryDetails;
            this.CommitHistoryViewModel = factory.CreateCommitHistoryViewModel(this.RepositoryDetails);
        }

        /// <inheritdoc />
        public ICommitHistoryViewModel CommitHistoryViewModel { get; }

        /// <inheritdoc />
        public string FriendlyName => this.RepositoryDetails.FriendlyName;

        /// <inheritdoc />
        public string RepositoryPath => this.RepositoryDetails.RepositoryPath;

        [Reactive]
        public IRepositoryDetails RepositoryDetails { get; set; }
    }
}