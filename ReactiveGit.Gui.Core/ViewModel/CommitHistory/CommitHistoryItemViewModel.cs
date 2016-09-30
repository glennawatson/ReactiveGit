namespace ReactiveGit.Gui.Core.ViewModel.CommitHistory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using ReactiveGit.Core.Model;
    using ReactiveGit.Gui.Core.ViewModel.Gravatar;

    using ReactiveUI;

    /// <summary>
    /// Represents a commit item in the history view model.
    /// </summary>
    public class CommitHistoryItemViewModel : ReactiveObject, IEquatable<CommitHistoryItemViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommitHistoryItemViewModel"/> class.
        /// </summary>
        /// <param name="commit">The commit the view model is related to.</param>
        public CommitHistoryItemViewModel(GitCommit commit)
        {
            this.GitCommit = commit;

            this.Gravatar = new GravatarViewModel { EmailAddress = this.GitCommit.CommitterEmail };
        }

        /// <summary>
        /// Gets the git commit.
        /// </summary>
        public GitCommit GitCommit { get; }

        /// <summary>
        /// Gets the gravatar logo. 
        /// </summary>
        public GravatarViewModel Gravatar { get; }

        /// <summary>
        /// Gets the name of the committer.
        /// </summary>
        public string CommiterName => this.GitCommit.Committer;

        /// <summary>
        /// Gets the short message.
        /// </summary>
        public string MessageShort => this.GitCommit.MessageShort;

        /// <summary>
        /// Gets the abbreviated SHA id.
        /// </summary>
        public string ShaShort => this.GitCommit.ShaShort;

        /// <summary>
        /// Gets the commit date and time.
        /// </summary>
        public DateTime CommitDateTime => this.GitCommit.DateTime;

        /// <summary>
        /// Compares the two sides, and returns if they are equal.
        /// </summary>
        /// <param name="left">The left side to compare.</param>
        /// <param name="right">The right side to compare.</param>
        /// <returns>If the two sides are equal.</returns>
        public static bool operator ==(CommitHistoryItemViewModel left, CommitHistoryItemViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Compares the two sides, and returns if they are not equal.
        /// </summary>
        /// <param name="left">The left side to compare.</param>
        /// <param name="right">The right side to compare.</param>
        /// <returns>If the two sides are not equal.</returns>
        public static bool operator !=(CommitHistoryItemViewModel left, CommitHistoryItemViewModel right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public bool Equals(CommitHistoryItemViewModel other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.GitCommit, other.GitCommit);
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

            var other = obj as CommitHistoryItemViewModel;
            return other != null && this.Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return this.GitCommit != null ? this.GitCommit.GetHashCode() : 0;
        }
    }
}
