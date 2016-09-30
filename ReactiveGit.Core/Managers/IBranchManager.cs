namespace ReactiveGit.Core.Managers
{
    using System;
    using System.Reactive;
    using System.Threading;
    using System.Threading.Tasks;

    using ReactiveGit.Core.Model;

    /// <summary>
    /// Manages and handling getting branch information.
    /// </summary>
    public interface IBranchManager
    {
        /// <summary>
        /// Gets a observable to the current branch.
        /// </summary>
        IObservable<GitBranch> CurrentBranch { get; }

        /// <summary>
        /// Checks out the specified branch.
        /// </summary>
        /// <param name="branch">The branch to check out.</param>
        /// <param name="force">If to force the branch change.</param>
        /// <returns>The observable.</returns>
        IObservable<Unit> CheckoutBranch(GitBranch branch, bool force = false);

        /// <summary>
        /// Gets the number of commits for a branch.
        /// </summary>
        /// <param name="branchName">The name of the branch.</param>
        /// <returns>The number of commits.</returns>
        int GetCommitCount(GitBranch branchName);

        /// <summary>
        /// Gets the commit message for a commit.
        /// </summary>
        /// <param name="commit">The commit to get the message for.</param>
        /// <returns>The message of the commit.</returns>
        string GetCommitMessageLong(GitCommit commit);

        /// <summary>
        /// Gets the command messages after the specified parent object.
        /// </summary>
        /// <param name="parent">The parent to get the commit messages for.</param>
        /// <returns>The commit messages</returns>
        IObservable<string> GetCommitMessagesAfterParent(GitCommit parent);

        /// <summary>
        /// Gets the commits for the specified branch.
        /// </summary>
        /// <param name="branch">The branch to get the commits for.</param>
        /// <param name="skip">Number of commits to skip.</param>
        /// <param name="limit">The number of commits to bring back.</param>
        /// <param name="logOptions">The options for how to order the git log.</param>
        /// <returns>A list of git commits.</returns>
        IObservable<GitCommit> GetCommitsForBranch(GitBranch branch, int skip, int limit, GitLogOptions logOptions);

        /// <summary>
        /// Get a list of both the local and remote branches.
        /// </summary>
        /// <returns>The local and remote branches.</returns>
        IObservable<GitBranch> GetLocalAndRemoteBranches();

        /// <summary>
        /// Gets a list of local branches.
        /// </summary>
        /// <returns>The local branches.</returns>
        IObservable<GitBranch> GetLocalBranches();

        /// <summary>
        /// Gets a remote branch of the specified branch.
        /// </summary>
        /// <param name="branch">The branch to get the </param>
        /// <param name="token">A cancellation token to stop the process.</param>
        /// <returns>The commit messages</returns>
        Task<GitBranch> GetRemoteBranch(GitBranch branch, CancellationToken token);

        /// <summary>
        /// Gets a list of remote branches.
        /// </summary>
        /// <returns>The remote branches.</returns>
        IObservable<GitBranch> GetRemoteBranches();

        /// <summary>
        /// Determines if there are any merge conflicts.
        /// </summary>
        /// <param name="token">A cancellation token to stop the process.</param>
        /// <returns>If there are any merge conflicts.</returns>
        Task<bool> IsMergeConflict(CancellationToken token);

        /// <summary>
        /// Determines if there are any changes in the working directory.
        /// </summary>
        /// <param name="token">A cancellation token to stop the process.</param>
        /// <returns>If there are changes in the working directory.</returns>
        Task<bool> IsWorkingDirectoryDirty(CancellationToken token);
    }
}