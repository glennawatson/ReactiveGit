namespace ReactiveGit.Process.Managers
{
    using System;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;

    using ReactiveGit.Core.Exceptions;
    using ReactiveGit.Core.Managers;
    using ReactiveGit.Core.Model;

    /// <summary>
    /// Implementation of the tag manager using command line git processes.
    /// </summary>
    public class TagManager : ITagManager
    {
        private readonly IGitProcessManager processManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagManager"/> class.
        /// </summary>
        /// <param name="processManager">The git process manager which will invoke git commands.</param>
        public TagManager(IGitProcessManager processManager)
        {
            this.processManager = processManager;
        }

        /// <inheritdoc />
        public IObservable<GitTag> GetTags(IScheduler scheduler = null)
        {
            string[] arguments = { "tag", "-l", "--format=\"%(refname:short)\u001f%(taggerdate:iso)\u001f%(objectname)\u001f%(objectname:short)\"" };
            return this.processManager.RunGit(arguments, scheduler: scheduler).Select(this.StringToGitTag);
        }

        /// <inheritdoc />
        public string GetMessage(GitTag gitTag)
        {
            string[] arguments = { "show", "--format=\"%B\"", gitTag.Name };
            var listValue = this.processManager.RunGit(arguments).ToList().Wait();

            return string.Join("\r\n", listValue).Trim(' ', '\r', '\n');
        }

        private GitTag StringToGitTag(string line)
        {
            string[] fields = line.Split('\u001f');

            if (fields.Length != 4)
            {
                throw new GitProcessException($"Cannot process tag entry {line}");
            }

            string name = fields[0];

            DateTime tagDate;
            DateTime.TryParse(fields[1], out tagDate);

            string sha = fields[2];
            string shaShort = fields[3];
            return new GitTag(this, name, shaShort, sha, tagDate);
        }
    }
}
