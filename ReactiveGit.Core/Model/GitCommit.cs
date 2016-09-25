namespace ReactiveGit.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using ReactiveGit.Core.Managers;

    /// <summary>
    /// A commit in GIT.
    /// </summary>
    [DebuggerDisplay("Id = {Sha}")]
    public class GitCommit : IEquatable<GitCommit>, IGitIdObject
    {
        private readonly IBranchManager branchManager;

        private readonly Lazy<string> messageLong;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitCommit"/> class.
        /// </summary>
        /// <param name="branchManager">The branch manager that owns the commit.</param>
        /// <param name="sha">The Sha Id for this commit.</param>
        /// <param name="messageShort">A short message about the commit.</param>
        /// <param name="dateTime">The date time of the commit.</param>
        /// <param name="author">The author of the commit.</param>
        /// <param name="authorEmail">The email of the author.</param>
        /// <param name="committer">The committer of the commit.</param>
        /// <param name="committerEmail">The email of the committer.</param>
        /// <param name="shaShort">The shorten version of the Sha hash.</param>
        /// <param name="parents">The parents of the commit.</param>
        public GitCommit(IBranchManager branchManager, string sha, string messageShort, DateTime dateTime, string author, string authorEmail, string committer, string committerEmail, string shaShort, IReadOnlyList<string> parents)
        {
            this.branchManager = branchManager;
            this.messageLong = new Lazy<string>(() => this.branchManager.GetCommitMessageLong(this));

            this.Sha = sha;
            this.MessageShort = messageShort;
            this.DateTime = dateTime;
            this.Author = author;
            this.Committer = committer;
            this.ShaShort = shaShort;
            this.Parents = parents;
            this.CommitterEmail = committerEmail;
            this.AuthorEmail = authorEmail;
        }

        /// <summary>
        /// Gets the Sha Id code.
        /// </summary>
        public string Sha { get; }

        /// <summary>
        /// Gets the short SHA value.
        /// </summary>
        public string ShaShort { get; }

        /// <summary>
        /// Gets the description of the commit.
        /// </summary>
        public string MessageShort { get; }

        /// <summary>
        /// Gets the full commit message.
        /// </summary>
        public string MessageLong => this.messageLong.Value;

        /// <summary>
        /// Gets the date time of the commit.
        /// </summary>
        public DateTime DateTime { get; }

        /// <summary>
        /// Gets the author of the commit.
        /// </summary>
        public string Author { get; }

        /// <summary>
        /// Gets the author's email.
        /// </summary>
        public string AuthorEmail { get; }

        /// <summary>
        /// Gets the commiter of the commit.
        /// </summary>
        public string Committer { get; }

        /// <summary>
        /// Gets the committer's email.
        /// </summary>
        public string CommitterEmail { get; }

        /// <summary>
        /// Gets a read only list of the parents of the commit.
        /// </summary>
        public IReadOnlyList<string> Parents { get; }

        /// <summary>
        /// Determines if two commits are equal to each other.
        /// </summary>
        /// <param name="left">The left side to compare.</param>
        /// <param name="right">The right side to compare.</param>
        /// <returns>If the commits are equal to each other.</returns>
        public static bool operator ==(GitCommit left, GitCommit right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Determines if two commits are not equal to each other.
        /// </summary>
        /// <param name="left">The left side to compare.</param>
        /// <param name="right">The right side to compare.</param>
        /// <returns>If the commits are not equal to each other.</returns>
        public static bool operator !=(GitCommit left, GitCommit right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// Determines if another instance of a commit is logically equal.
        /// </summary>
        /// <param name="other">The other commit.</param>
        /// <returns>If they are logically equal or not.</returns>
        public bool Equals(GitCommit other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            return ReferenceEquals(this, other) || string.Equals(this.Sha, other.Sha);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == this.GetType() && this.Equals((GitCommit)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return this.Sha?.GetHashCode() ?? 0;
        }
    }
}