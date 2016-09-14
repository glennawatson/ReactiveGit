namespace ReactiveGit.Gui.Base.ViewModel.CommitHistory
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using ReactiveGit.Model;

    /// <summary>
    /// A view model for a GIT repository.
    /// </summary>
    public interface ICommitHistoryViewModel : IChildRepositoryDocumentViewModel
    {
        /// <summary>
        /// Gets a collection of commit history.
        /// </summary>
        IEnumerable<GitCommit> CommitHistory { get; }

        /// <summary>
        /// Gets the current branch that the repository is in.
        /// </summary>
        GitBranch CurrentBranch { get; }

        /// <summary>
        /// Gets a command that refreshes the values from GIT.
        /// </summary>
        ICommand Refresh { get; }
    }
}
