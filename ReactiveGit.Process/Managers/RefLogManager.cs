namespace ReactiveGit.Managers
{
    using System;
    using System.Reactive.Linq;

    using ReactiveGit.Exceptions;
    using ReactiveGit.Model;

    /// <summary>
    /// Manages handling ref log instances.
    /// </summary>
    public class RefLogManager : IRefLogManager
    {
        private readonly IGitProcessManager gitProcessManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefLogManager"/> class.
        /// </summary>
        /// <param name="gitProcessManager">The git process to use.</param>
        public RefLogManager(IGitProcessManager gitProcessManager)
        {
            this.gitProcessManager = gitProcessManager;
        }

        /// <inheritdoc />
        public IObservable<GitRefLog> GetRefLog(GitBranch branch)
        {
            string[] arguments = new[] { "reflog", "--format=\"%h\u001f%gd\u001f%gs\u001f%ci\"", branch.FriendlyName };

            return this.gitProcessManager.RunGit(arguments).Select(this.StringToRefLog);
        }

        private GitRefLog StringToRefLog(string line)
        {
            string[] fields = line.Split('\u001f');

            if (fields.Length != 4)
            {
                throw new GitProcessException($"Cannot process ref log entry {line}");
            }

            string shaShort = fields[0];
            string[] refLogSubject = fields[2].Split(new[] { ':' }, 2);
            string operation = refLogSubject[0];
            string condenseText = refLogSubject[1];

            DateTime commitDate;
            DateTime.TryParse(fields[3], out commitDate);

            return new GitRefLog(shaShort, operation, condenseText, commitDate);
        }
    }
}
