namespace ReactiveGit.Process.Managers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reactive;
    using System.Reactive.Concurrency;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Reflection;

    using ReactiveGit.Core.Exceptions;
    using ReactiveGit.Core.ExtensionMethods;
    using ReactiveGit.Core.Managers;
    using ReactiveGit.Core.Model;

    /// <summary>
    /// Class responsible for handling GIT rebases.
    /// </summary>
    public class RebaseManager : IRebaseManager
    {
        private readonly IBranchManager branchManager;

        private readonly IGitProcessManager gitProcess;

        /// <summary>
        /// Initializes a new instance of the <see cref="RebaseManager" /> class.
        /// </summary>
        /// <param name="processManager">The process manager which invokes GIT commands.</param>
        /// <param name="branchManager">The branch manager which will get's GIT branch information.</param>
        public RebaseManager(IGitProcessManager processManager, IBranchManager branchManager)
        {
            this.gitProcess = processManager;
            this.branchManager = branchManager;
        }

        /// <summary>
        /// Gets the writers names.
        /// </summary>
        /// <param name="rebaseWriter">The rebase name.</param>
        /// <param name="commentWriter">The comment name.</param>
        /// <returns>The commit.</returns>
        public static bool GetWritersName(out string rebaseWriter, out string commentWriter)
        {
            rebaseWriter = null;
            commentWriter = null;

            try
            {
                var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
                string location = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);

                if (File.Exists(location) == false)
                {
                    location = Assembly.GetExecutingAssembly().Location;

                    if (string.IsNullOrWhiteSpace(location))
                    {
                        throw new GitProcessException("Cannot find location of writers");
                    }

                    location = Uri.UnescapeDataString(location);
                }

                if (string.IsNullOrWhiteSpace(location))
                {
                    return false;
                }

                string directoryName = Path.GetDirectoryName(location);

                if (string.IsNullOrWhiteSpace(directoryName))
                {
                    return false;
                }

                rebaseWriter = Path.Combine(directoryName, "rebasewriter.exe").Replace(@"\", "/");
                commentWriter = Path.Combine(directoryName, "commentWriter.exe").Replace(@"\", "/");
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (PathTooLongException)
            {
                return false;
            }
        }

        /// <inheritdoc />
        public IObservable<Unit> Abort(IScheduler scheduler = null)
        {
            return this.gitProcess.RunGit(new[] { "rebase --abort" }, showInOutput: true, scheduler: scheduler).WhenDone();
        }

        /// <inheritdoc />
        public IObservable<Unit> Continue(string commitMessage, IScheduler scheduler = null)
        {
            return Observable.Create<Unit>(
                observer =>
                    {
                        string rewriterName;
                        string commentWriterName;
                        if (GetWritersName(out rewriterName, out commentWriterName) == false)
                        {
                            observer.OnError(new GitProcessException("Cannot get valid paths to GIT parameters"));
                        }

                        string fileName = Path.GetTempFileName();
                        File.WriteAllText(fileName, commitMessage);

                        IList<string> gitArguments = new List<string>
                                                         {
                                                             $"-c \"core.editor=\'{commentWriterName}\'\"",
                                                             "rebase --continue"
                                                         };

                        var environmentVariables = new Dictionary<string, string> { { "COMMENT_FILE_NAME", fileName } };

                        IDisposable running = this.gitProcess.RunGit(gitArguments, environmentVariables, showInOutput: true, scheduler: scheduler).Subscribe(
                            _ => { },
                            observer.OnError,
                            () =>
                            {
                                observer.OnNext(Unit.Default);
                                observer.OnCompleted();
                            });

                        return Disposable.Create(() => running?.Dispose());
                    });
        }

        /// <inheritdoc />
        public IObservable<bool> HasConflicts(IScheduler scheduler = null)
        {
            return this.branchManager.IsMergeConflict(scheduler);
        }

        /// <inheritdoc />
        public bool IsRebaseHappening()
        {
            bool isFile = Directory.Exists(Path.Combine(this.gitProcess.RepositoryPath, ".git/rebase-apply"));

            return isFile || Directory.Exists(Path.Combine(this.gitProcess.RepositoryPath, ".git/rebase-merge"));
        }

        /// <inheritdoc />
        public IObservable<Unit> Rebase(GitBranch parentBranch, IScheduler scheduler = null)
        {
            return Observable.Create<Unit>(
                async (observer, token) =>
                    {
                        if (await this.branchManager.IsWorkingDirectoryDirty())
                        {
                            observer.OnError(
                                new GitProcessException("The working directory is dirty. There are uncommited files."));
                        }

                        IList<string> gitArguments = new List<string> { $"rebase -i  {parentBranch.FriendlyName}" };

                        this.gitProcess.RunGit(gitArguments, showInOutput: true, scheduler: scheduler).Subscribe(
                            _ => { }, 
                            observer.OnError,
                            () =>
                                {
                                    observer.OnNext(Unit.Default);
                                    observer.OnCompleted();
                                }, token);
                    });
        }

        /// <inheritdoc />
        public IObservable<Unit> Skip(IScheduler scheduler = null)
        {
            return this.gitProcess.RunGit(new[] { "rebase --skip" }, showInOutput: true, scheduler: scheduler).WhenDone();
        }

        /// <inheritdoc />
        public IObservable<Unit> Squash(string newCommitMessage, GitCommit startCommit, IScheduler scheduler = null)
        {
            return Observable.Create<Unit>(
                async (observer, token) =>
                    {
                        if (await this.branchManager.IsWorkingDirectoryDirty())
                        {
                            observer.OnError(
                                new GitProcessException("The working directory is dirty. There are uncommited files."));
                        }

                        string rewriterName;
                        string commentWriterName;
                        if (GetWritersName(out rewriterName, out commentWriterName) == false)
                        {
                            observer.OnError(new GitProcessException("Cannot get valid paths to GIT parameters"));
                        }

                        string fileName = Path.GetTempFileName();
                        File.WriteAllText(fileName, newCommitMessage);

                        var environmentVariables = new Dictionary<string, string> { { "COMMENT_FILE_NAME", fileName } };

                        IList<string> gitArguments = new List<string>
                                                         {
                                                             $"-c \"sequence.editor=\'{rewriterName}\'\"",
                                                             $"-c \"core.editor=\'{commentWriterName}\'\"",
                                                             $"rebase -i  {startCommit.Sha}"
                                                         };

                        this.gitProcess.RunGit(gitArguments, environmentVariables, showInOutput: true, scheduler: scheduler).Subscribe(
                            _ => { },
                            observer.OnError,
                            () =>
                            {
                                observer.OnNext(Unit.Default);
                                observer.OnCompleted();
                            }, token);
                    });
        }
    }
}