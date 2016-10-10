namespace ReactiveGit.Core.Managers
{
    using System;
    using System.Reactive;
    using System.Reactive.Concurrency;
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
        /// <param name="scheduler">The scheduler to schedule the task on.</param>
        /// <returns>The observable.</returns>
        IObservable<Unit> CheckoutBranch(GitBranch branch, bool force = false, IScheduler scheduler = null);

        /// <summary>
        /// Gets the number of commits for a branch.
        /// </summary>
        /// <param name="branchName">The name of the branch.</param>
        /// <param name="scheduler">The scheduler to schedule the task on.</param>
        /// <returns>The number of commits.</returns>
        int GetCommitCount(GitBranch branchName, IScheduler scheduler = null);

        /// <summary>
        /// Gets the commit message for a commit.
        /// </summary>
        /// <param name="commit">The commit to get the message for.</param>
        /// <param name="scheduler">The scheduler to schedule the task on.</param>
        /// <returns>The message of the commit.</returns>
        string GetCommitMessageLong(GitCommit commit, IScheduler scheduler = null);

        /// <summary>
        /// Gets the command messages after the specified parent object.
        /// </summary>
        /// <param name="parent">The parent to get the commit messages for.</param>
        /// <param name="scheduler">The scheduler to schedule the task on.</param>
        /// <returns>The commit messages</returns>
        IObservable<string> GetCommitMessagesAfterParent(GitCommit parent, IScheduler scheduler = null);

        /// <summary>
        /// Gets the commits for the specified branch.
        /// </summary>
        /// <param name="branch">The branch to get the commits for.</param>
        /// <param name="skip">Number of commits to skip.</param>
        /// <param name="limit">The number of commits to bring back.</param>
        /// <param name="logOptions">The options for how to order the git log.</param>
        /// <param name="scheduler">The scheduler to schedule the task on.</param>
        /// <returns>A list of git commits.</returns>
        IObservable<GitCommit> GetCommitsForBranch(GitBranch branch, int skip, int limit, GitLogOptions logOptions, IScheduler scheduler = null);

        /// <summary>
        /// Get a list of both the local and remote branches.
        /// </summary>
        /// <param name="scheduler">The scheduler to schedule the task on.</param>
        /// <returns>The local and remote branches.</returns>
        IObservable<GitBranch> GetLocalAndRemoteBranches(IScheduler scheduler = null);

        /// <summary>
        /// Gets a list of local branches.
        /// </summary>
        /// <param name="scheduler">The scheduler to schedule the task on.</param>
        /// <returns>The local branches.</returns>
        IObservable<GitBranch> GetLocalBranches(IScheduler scheduler = null);

        /// <summary>
        /// Gets a remote branch of the specified branch.
        /// </summary>
        /// <param name="branch">The branch to get the </param>
        /// <param name="scheduler">The scheduler to schedule the task on.</param>
        /// <returns>The commit messages</returns>
        IObservable<GitBranch> GetRemoteBranch(GitBranch branch, IScheduler scheduler = null);

        /// <summary>
        /// Gets a list of remote branches.
        /// </summary>
        /// <param name="scheduler">The scheduler to schedule the task on.</param>
        /// <returns>The remote branches.</returns>
        IObservable<GitBranch> GetRemoteBranches(IScheduler scheduler = null);

        /// <summary>
        /// Determines if there are any merge conflicts.
        /// </summary>
        /// <param name="scheduler">The scheduler to schedule the task on.</param>
        /// <returns>If there are any merge conflicts.</returns>
        IObservable<bool> IsMergeConflict(IScheduler scheduler = null);

        /// <summary>
        /// Determines if there are any changes in the working directory.
        /// </summary>
        /// <param name="scheduler">The scheduler to schedule the task on.</param>
        /// <returns>If there are changes in the working directory.</returns>
        IObservable<bool> IsWorkingDirectoryDirty(IScheduler scheduler = null);
    }
}