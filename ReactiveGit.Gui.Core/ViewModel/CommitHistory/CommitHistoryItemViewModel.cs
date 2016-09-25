namespace ReactiveGit.Gui.Core.ViewModel.CommitHistory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using ReactiveGit.Core.Model;

    using ReactiveUI;

    public class CommitHistoryItemViewModel : ReactiveObject
    {
        private readonly GitCommit commit;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommitHistoryItemViewModel"/> class.
        /// </summary>
        /// <param name="commit">The commit the view model is related to.</param>
        public CommitHistoryItemViewModel(GitCommit commit)
        {
            this.commit = commit;

            this.Gravatar = new GravatarViewModel { EmailAddress = this.commit.Author };
        }

        /// <summary>
        /// Gets the gravatar logo. 
        /// </summary>
        public GravatarViewModel Gravatar { get; }

        /// <summary>
        /// Gets the name of the committer.
        /// </summary>
        public string CommiterName => this.commit.Committer;

        /// <summary>
        /// Gets the short message.
        /// </summary>
        public string MessageShort => this.commit.MessageShort;

        /// <summary>
        /// Gets the abbreviated SHA id.
        /// </summary>
        public string ShaShort => this.commit.ShaShort;

        /// <summary>
        /// Gets the commit date and time.
        /// </summary>
        public DateTime CommitDateTime => this.commit.DateTime;
    }
}
