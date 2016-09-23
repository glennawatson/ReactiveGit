namespace ReactiveGit.Gui.Base.ViewModel.CommitHistory
{
    using System.Collections.Generic;

    using ReactiveGit.Model;

    /// <summary>
    /// A view model for a GIT repository.
    /// </summary>
    public interface ICommitHistoryViewModel : IGitObjectViewModel
    {
        /// <summary>
        /// Gets a collection of commit history.
        /// </summary>
        IEnumerable<GitCommit> CommitHistory { get; }
    }
}