namespace ReactiveGit.Gui.Base.ViewModel.Repository
{
    using ReactiveGit.Gui.Base.Model.Branches;
    using ReactiveGit.Gui.Base.ViewModel.Branches;
    using ReactiveGit.Gui.Base.ViewModel.CommitHistory;
    using ReactiveGit.Gui.Base.ViewModel.RefLog;

    /// <summary>
    /// A document for the repository.
    /// </summary>
    public interface IRepositoryDocumentViewModel
    {
        /// <summary>
        /// Gets the view model associated with the commit history of the repository.
        /// </summary>
        ICommitHistoryViewModel CommitHistoryViewModel { get; }

        /// <summary>
        /// Gets the view model associated with branch operations.
        /// </summary>
        IBranchViewModel BranchViewModel { get; }

        /// <summary>
        /// Gets the view model associated with the ref log.
        /// </summary>
        IRefLogViewModel RefLogViewModel { get; }

        /// <summary>
        /// Gets a user friendly name of the repository.
        /// </summary>
        string FriendlyName { get; }

        /// <summary>
        /// Gets the path to the repository.
        /// </summary>
        string RepositoryPath { get; }

        /// <summary>
        /// Gets or sets the selected branch node.
        /// </summary>
        BranchNode SelectedBranchNode { get; set; }
    }
}