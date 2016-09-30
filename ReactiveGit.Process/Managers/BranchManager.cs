namespace ReactiveGit.Process.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using ReactiveGit.Core.ExtensionMethods;
    using ReactiveGit.Core.Managers;
    using ReactiveGit.Core.Model;

    /// <summary>
    /// Helper which manages branch history.
    /// </summary>
    public class BranchManager : IBranchManager
    {
        private readonly Subject<GitBranch> currentBranch = new Subject<GitBranch>();

        private readonly IGitProcessManager gitProcessManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchManager" /> class.
        /// </summary>
        /// <param name="gitProcessManager">The git process to use.</param>
        public BranchManager(IGitProcessManager gitProcessManager)
        {
            this.gitProcessManager = gitProcessManager;
        }

        /// <inheritdoc />
        public IObservable<GitBranch> CurrentBranch
        {
            get
            {
                this.GetCurrentCheckedOutBranch();
                return this.currentBranch;
            }
        }

        /// <inheritdoc />
        public IObservable<Unit> CheckoutBranch(GitBranch branch, bool force = false)
        {
            IList<string> arguments = new List<string> { $"checkout {branch.FriendlyName}" };

            if (force)
            {
                arguments.Add("-f");
            }

            IObservable<Unit> observable = this.gitProcessManager.RunGit(arguments).WhenDone();
            return observable.Finally(() => this.currentBranch.OnNext(branch));
        }

        /// <inheritdoc />
        public int GetCommitCount(GitBranch branchName)
        {
            return
                Convert.ToInt32(
                    this.gitProcessManager.RunGit(new[] { $"rev-list --count {branchName.FriendlyName}" }).ToList().Wait
                        ().First());
        }

        /// <inheritdoc />
        public string GetCommitMessageLong(GitCommit commit)
        {
            IList<string> result =
                this.gitProcessManager.RunGit(new[] { "log", "--format=%B", "-n 1", commit.Sha }).Select(
                    x => x.Trim().Trim('\r', '\n')).ToList().Wait();
            return string.Join("\r\n", result).Trim().Trim('\r', '\n', ' ');
        }

        /// <inheritdoc />
        public IObservable<string> GetCommitMessagesAfterParent(GitCommit parent)
        {
            return Observable.Create<string>(
                async (observer, token) =>
                    {
                        IEnumerable<string> arguments =
                            this.ExtractLogParameter(
                                await this.CurrentBranch.LastOrDefaultAsync(),
                                0,
                                0,
                                GitLogOptions.None,
                                $"{parent.Sha}..HEAD");
                        this.gitProcessManager.RunGit(arguments).Select(
                            x => this.ConvertStringToGitCommit(x).MessageLong.Trim('\r', '\n')).Subscribe(
                            observer.OnNext,
                            observer.OnCompleted,
                            token);
                    });
        }

        /// <inheritdoc />
        public IObservable<GitCommit> GetCommitsForBranch(
            GitBranch branch,
            int skip,
            int limit,
            GitLogOptions logOptions)
        {
            string[] arguments =
                new[] { "log" }.Concat(this.ExtractLogParameter(branch, skip, limit, logOptions, "HEAD")).ToArray();

            return this.gitProcessManager.RunGit(arguments).Select(this.ConvertStringToGitCommit);
        }

        /// <inheritdoc />
        public IObservable<GitBranch> GetLocalAndRemoteBranches()
        {
            return this.GetLocalBranches().Merge(this.GetRemoteBranches());
        }

        /// <inheritdoc />
        public IObservable<GitBranch> GetLocalBranches()
        {
            return
                this.gitProcessManager.RunGit(new[] { "branch" }).Select(
                    line => new GitBranch(line.Substring(2), false, line[0] == '*'));
        }

        /// <inheritdoc />
        public Task<GitBranch> GetRemoteBranch(GitBranch branch, CancellationToken token)
        {
            return Task.FromResult<GitBranch>(null);
        }

        /// <inheritdoc />
        public IObservable<GitBranch> GetRemoteBranches()
        {
            return this.gitProcessManager.RunGit(new[] { "branch" }).Select(
                line =>
                    {
                        int arrowPos = line.IndexOf(" -> ", StringComparison.InvariantCulture);
                        string branch = line;
                        if (arrowPos != -1)
                        {
                            branch = line.Substring(0, arrowPos);
                        }

                        return new GitBranch(branch.Trim(), true, false);
                    });
        }

        /// <inheritdoc />
        public async Task<bool> IsMergeConflict(CancellationToken token)
        {
            return await this.gitProcessManager.RunGit(new[] { "ls-files", "-u" }).Any();
        }

        /// <inheritdoc />
        public async Task<bool> IsWorkingDirectoryDirty(CancellationToken token)
        {
            string[] arguments = { "status", "--porcelain", "--ignore-submodules=dirty", "--untracked-files=all" };

            return await this.gitProcessManager.RunGit(arguments).Any();
        }

        private static void GenerateFormat(IList<string> arguments)
        {
            var formatString = new StringBuilder("--format=%H\u001f%h\u001f%P\u001f");
            formatString.Append("%ci");
            formatString.Append("\u001f%cn\u001f%ce\u001f%an\u001f%ae\u001f%d\u001f%s\u001f");
            arguments.Add(formatString.ToString());
            arguments.Add("--decorate=full");
            arguments.Add("--date=iso");
        }

        private GitCommit ConvertStringToGitCommit(string line)
        {
            string[] fields = line.Split('\u001f');

            if (fields.Length != 11)
            {
                return null;
            }

            string changeset = fields[0];
            string changesetShort = fields[1];
            string[] parents =
                fields[2].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(
                    x => x.Trim('\r', '\n').Trim()).ToArray();
            DateTime commitDate;
            DateTime.TryParse(fields[3], out commitDate);
            string committer = fields[4];
            string commiterEmail = fields[5];
            string author = fields[6];
            string authorEmail = fields[7];
            string refs = fields[8];
            string messageShort = fields[9];

            return new GitCommit(
                       this,
                       changeset,
                       messageShort,
                       commitDate,
                       author,
                       authorEmail,
                       committer,
                       commiterEmail,
                       changesetShort,
                       parents);
        }

        private IEnumerable<string> ExtractLogParameter(
            GitBranch branch,
            int skip,
            int limit,
            GitLogOptions logOptions,
            string revisionRange)
        {
            IList<string> arguments = new List<string>();

            arguments.Add($"{revisionRange} ");

            if (branch != null)
            {
                arguments.Add($"--branches={branch.FriendlyName} ");
            }

            if (skip > 0)
            {
                arguments.Add($"--skip={skip}");
            }

            if (limit > 0)
            {
                arguments.Add($"--max-count={limit}");
            }

            arguments.Add("--full-history");

            if (logOptions.HasFlag(GitLogOptions.TopologicalOrder))
            {
                arguments.Add("--topo-order");
            }

            if (!logOptions.HasFlag(GitLogOptions.IncludeMerges))
            {
                arguments.Add("--no-merges");
                arguments.Add("--first-parent");
            }

            GenerateFormat(arguments);

            if (logOptions.HasFlag(GitLogOptions.BranchOnlyAndParent))
            {
                var ignoreBranches = new StringBuilder("--not ");

                IList<GitBranch> branches = this.GetLocalBranches().ToList().Wait();

                foreach (GitBranch testBranch in branches)
                {
                    if (testBranch != branch)
                    {
                        ignoreBranches.Append($"{testBranch.FriendlyName} ");
                    }
                }

                arguments.Add($" {ignoreBranches} -- ");
            }

            return arguments;
        }

        private void GetCurrentCheckedOutBranch()
        {
            this.gitProcessManager.RunGit(new[] { "branch" }).Where(x => x.StartsWith("*")).Select(
                line => new GitBranch(line.Substring(2), false, true)).Subscribe(this.currentBranch.OnNext);
        }
    }
}