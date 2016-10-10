namespace ReactiveGit.Core.Model
{
    using System;

    using ReactiveGit.Core.Managers;

    /// <summary>
    /// Represents a git tag.
    /// </summary>
    public class GitTag : IGitIdObject
    {
        private readonly Lazy<string> message;

        private readonly ITagManager tagManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitTag"/> class.
        /// </summary>
        /// <param name="tagManager">The tag manager to get tag information.</param>
        /// <param name="name">The name of the tag.</param>
        /// <param name="shaShort">A abbreviated SHA id.</param>
        /// <param name="sha">The SHA id.</param>
        /// <param name="dateTime">The date time the tag was created.</param>
        public GitTag(ITagManager tagManager, string name, string shaShort, string sha, DateTime dateTime)
        {
            this.Name = name;
            this.tagManager = tagManager;
            this.Sha = sha;
            this.ShaShort = shaShort;
            this.DateTime = dateTime;

            this.message = new Lazy<string>(() => this.tagManager.GetMessage(this));
        }

        /// <inheritdoc />
        public string Sha { get; }

        /// <inheritdoc />
        public string ShaShort { get; }

        /// <summary>
        /// Gets the date time the tag was created.
        /// </summary>
        public DateTime DateTime { get; }

        /// <summary>
        /// Gets a message about the tag.
        /// </summary>
        public string Message => this.message.Value;

        /// <summary>
        /// Gets the name of the tag.
        /// </summary>
        public string Name { get; }
    }
}
