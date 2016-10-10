namespace ReactiveGit.Core.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Concurrency;
    using System.Text;
    using System.Threading.Tasks;

    using ReactiveGit.Core.Model;

    /// <summary>
    /// A manager for handling tags.
    /// </summary>
    public interface ITagManager
    {
        /// <summary>
        /// Get the tags inside the git repository.
        /// </summary>
        /// <param name="scheduler">The scheduler to schedule on.</param>
        /// <returns>A observable of the tags.</returns>
        IObservable<GitTag> GetTags(IScheduler scheduler = null);

        /// <summary>
        /// Gets the message for a tag.
        /// </summary>
        /// <param name="gitTag">The tag to get a message for.</param>
        /// <returns>The message for the tag.</returns>
        string GetMessage(GitTag gitTag);
    }
}
