// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using ReactiveGit.Gui.Core.ViewModel.Gravatar;
using ReactiveGit.Library.Core.Model;
using ReactiveUI;

namespace ReactiveGit.Gui.Core.ViewModel.CommitHistory
{
    /// <summary>
    /// Represents a commit item in the history view model.
    /// </summary>
    public class CommitHistoryItemViewModel : ReactiveObject, IEquatable<CommitHistoryItemViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommitHistoryItemViewModel" /> class.
        /// </summary>
        /// <param name="commit">The commit the view model is related to.</param>
        public CommitHistoryItemViewModel(GitCommit commit)
        {
            GitCommit = commit;

            Gravatar = new GravatarViewModel { EmailAddress = GitCommit.CommitterEmail };
        }

        /// <summary>
        /// Gets the commit date and time.
        /// </summary>
        public DateTime CommitDateTime => GitCommit.DateTime;

        /// <summary>
        /// Gets the name of the committer.
        /// </summary>
        public string CommiterName => GitCommit.Committer;

        /// <summary>
        /// Gets the git commit.
        /// </summary>
        public GitCommit GitCommit { get; }

        /// <summary>
        /// Gets the gravatar logo.
        /// </summary>
        public GravatarViewModel Gravatar { get; }

        /// <summary>
        /// Gets the short message.
        /// </summary>
        public string MessageShort => GitCommit.MessageShort;

        /// <summary>
        /// Gets the abbreviated SHA id.
        /// </summary>
        public string ShaShort => GitCommit.ShaShort;

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

            return Equals(GitCommit, other.GitCommit);
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
            return (other != null) && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return GitCommit != null ? GitCommit.GetHashCode() : 0;
        }
    }
}