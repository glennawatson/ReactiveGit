namespace ReactiveGit.Core.Managers
{
    using System;
    using System.Reactive;
    using System.Threading;
    using System.Threading.Tasks;

    using ReactiveGit.Core.Model;

    /// <summary>
    /// Manager relating to rebasing activities.
    /// </summary>
    public interface IRebaseManager
    {
        /// <summary>
        /// Aborts a attempt to squash/rebase.
        /// </summary>
        /// <returns>Details about the operation.</returns>
        IObservable<Unit> Abort();

        /// <summary>
        /// Indicates we want to continue after a conflict was found.
        /// </summary>
        /// <param name="commitMessage">The commit message to use.</param>
        /// <returns>Details about the operation.</returns>
        IObservable<Unit> Continue(string commitMessage);

        /// <summary>
        /// Determines if there are any conflicts.
        /// </summary>
        /// <returns>True if there is a conflict, false otherwise.</returns>
        IObservable<bool> HasConflicts();

        /// <summary>
        /// Determines if there is currently a rebase in operation.
        /// </summary>
        /// <returns>True if rebase in operation, false otherwise.</returns>
        bool IsRebaseHappening();

        /// <summary>
        /// Performs the squash action.
        /// </summary>
        /// <param name="parentBranch">The branch parent to parent the rebase/squash from.</param>
        /// <returns>Details about the commit.</returns>
        IObservable<Unit> Rebase(GitBranch parentBranch);

        /// <summary>
        /// Skips the specified commit in a rebase.
        /// </summary>
        /// <returns>Details about the operation.</returns>
        IObservable<Unit> Skip();

        /// <summary>
        /// Performs the squash action.
        /// </summary>
        /// <param name="newCommitMessage">The new commit message for the squashed commit.</param>
        /// <param name="startCommit">The commit to start the rebase/squash from.</param>
        /// <returns>Details about the commit.</returns>
        IObservable<Unit> Squash(string newCommitMessage, GitCommit startCommit);
    }
}