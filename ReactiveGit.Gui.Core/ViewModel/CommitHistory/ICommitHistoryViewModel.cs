namespace ReactiveGit.Gui.Core.ViewModel.CommitHistory
{
    using System.Collections.Generic;

    using ReactiveGit.Gui.Core.ViewModel.GitObject;

    /// <summary>
    /// A view model for a GIT repository.
    /// </summary>
    public interface ICommitHistoryViewModel : IGitObjectViewModel
    {
        /// <summary>
        /// Gets a collection of commit history.
        /// </summary>
        IEnumerable<CommitHistoryItemViewModel> CommitHistory { get; }
    }
}