namespace ReactiveGit.Process.Managers
{
    using System;
    using System.Reactive.Linq;

    using ReactiveGit.Core.Exceptions;
    using ReactiveGit.Core.Managers;
    using ReactiveGit.Core.Model;

    /// <summary>
    /// Manages handling ref log instances.
    /// </summary>
    public class RefLogManager : IRefLogManager
    {
        private readonly IGitProcessManager gitProcessManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefLogManager" /> class.
        /// </summary>
        /// <param name="gitProcessManager">The git process to use.</param>
        public RefLogManager(IGitProcessManager gitProcessManager)
        {
            this.gitProcessManager = gitProcessManager;
        }

        /// <inheritdoc />
        public IObservable<GitRefLog> GetRefLog(GitBranch branch)
        {
            string[] arguments = { "reflog", "--format=\"%H\u001f%h\u001f%gd\u001f%gs\u001f%ci\"", branch.FriendlyName };

            return this.gitProcessManager.RunGit(arguments).Select(StringToRefLog);
        }

        private static GitRefLog StringToRefLog(string line)
        {
            string[] fields = line.Split('\u001f');

            if (fields.Length != 5)
            {
                throw new GitProcessException($"Cannot process ref log entry {line}");
            }

            string sha = fields[0];
            string shaShort = fields[1];
            string[] refLogSubject = fields[3].Split(new[] { ':' }, 2);
            string operation = refLogSubject[0];
            string condenseText = refLogSubject[1];

            DateTime commitDate;
            DateTime.TryParse(fields[4], out commitDate);

            return new GitRefLog(sha, shaShort, operation, condenseText, commitDate);
        }
    }
}