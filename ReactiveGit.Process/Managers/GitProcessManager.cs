// <copyright file="GitProcessManager.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Process.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reactive;
    using System.Reactive.Concurrency;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using ReactiveGit.Core.Exceptions;
    using ReactiveGit.Core.Managers;
    using ReactiveGit.Process.Helpers;

    /// <summary>
    /// Manages and starts GIT processes.
    /// </summary>
    public sealed class GitProcessManager : IGitProcessManager, IDisposable
    {
        private static readonly SemaphoreSlim RepoLimiterSemaphore = new SemaphoreSlim(1, 1);

        private readonly Subject<string> gitOutput = new Subject<string>();

        private readonly Subject<Unit> gitUpdated = new Subject<Unit>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GitProcessManager" /> class.
        /// </summary>
        /// <param name="repoDirectory">The location of the GIT repository.</param>
        public GitProcessManager(string repoDirectory)
        {
            this.RepositoryPath = repoDirectory;
        }

        /// <inheritdoc />
        public IObservable<string> GitOutput => this.gitOutput;

        /// <inheritdoc />
        public IObservable<Unit> GitUpdated => this.gitUpdated;

        /// <inheritdoc />
        public string RepositoryPath { get; }

        /// <inheritdoc />
        public IObservable<string> RunGit(
            IEnumerable<string> gitArgumentsEnumerable,
            IDictionary<string, string> extraEnvironmentVariables = null,
            [CallerMemberName] string callerMemberName = null,
            bool includeStandardArguments = true,
            bool showInOutput = false,
            IScheduler scheduler = null)
        {
            scheduler = scheduler ?? TaskPoolScheduler.Default;
            return Observable.Create<string>(
                (observer) =>
                    {
                        var d = new CompositeDisposable();

                        IDisposable schedule = scheduler.ScheduleAsync(
                            async (ctrl, token) =>
                                {
                                    string gitArguments = string.Join(" ", gitArgumentsEnumerable);
                                    if (includeStandardArguments)
                                    {
                                        gitArguments =
                                            $"--no-pager -c color.branch=false -c color.diff=false -c color.status=false -c diff.mnemonicprefix=false -c core.quotepath=false {gitArguments}";
                                    }

                                    if (showInOutput)
                                    {
                                        this.gitOutput.OnNext($"execute: git {gitArguments}");
                                    }

                                    using (Process process = CreateGitProcess(gitArguments, this.RepositoryPath))
                                    {
                                        if (extraEnvironmentVariables != null)
                                        {
                                            foreach (KeyValuePair<string, string> kvp in extraEnvironmentVariables)
                                            {
                                                process.StartInfo.EnvironmentVariables.Add(kvp.Key, kvp.Value);
                                            }
                                        }

                                        var errorOutput = new StringBuilder();
                                        process.ErrorDataReceived += (sender, e) =>
                                            {
                                                if (e.Data == null)
                                                {
                                                    return;
                                                }

                                                if (showInOutput)
                                                {
                                                    this.gitOutput.OnNext(e.Data);
                                                }

                                                errorOutput.AppendLine(e.Data);
                                                observer.OnNext(e.Data);
                                            };

                                        process.OutputDataReceived += (sender, e) =>
                                            {
                                                if (e.Data == null)
                                                {
                                                    return;
                                                }

                                                if (showInOutput)
                                                {
                                                    this.gitOutput.OnNext(e.Data);
                                                }

                                                errorOutput.AppendLine(e.Data);
                                                observer.OnNext(e.Data);
                                            };

                                        if (token.IsCancellationRequested)
                                        {
                                            observer.OnCompleted();
                                            return Disposable.Empty;
                                        }

                                        int returnValue = await RunProcessAsync(process, token);

                                        if (returnValue != 0)
                                        {
                                            observer.OnError(new GitProcessException(gitArguments, errorOutput.ToString()));
                                        }

                                        observer.OnCompleted();

                                        this.gitUpdated.OnNext(Unit.Default);

                                        return Disposable.Empty;
                                    }
                                });

                        d.Add(schedule);
                        return d;
                    });
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.gitOutput?.Dispose();
            this.gitUpdated?.Dispose();
        }

        private static Process CreateGitProcess(string arguments, string repoDirectory)
        {
            string gitInstallationPath = GitHelper.GetGitInstallationPath();
            string pathToGit = Path.Combine(Path.Combine(gitInstallationPath, @"bin\git.exe"));
            return new Process
            {
                StartInfo =
                               {
                                   CreateNoWindow = true,
                                   UseShellExecute = false,
                                   RedirectStandardInput = true,
                                   RedirectStandardOutput = true,
                                   RedirectStandardError = true,
                                   FileName = pathToGit,
                                   Arguments = arguments,
                                   WorkingDirectory = repoDirectory,
                                   StandardErrorEncoding = Encoding.UTF8,
                                   StandardOutputEncoding = Encoding.UTF8
                               },
                EnableRaisingEvents = true
            };
        }

        private static async Task<int> RunProcessAsync(Process process, CancellationToken token)
        {
            await RepoLimiterSemaphore.WaitAsync(token).ConfigureAwait(false);

            try
            {
                bool started = process.Start();
                if (!started)
                {
                    // you may allow for the process to be re-used (started = false) 
                    // but I'm not sure about the guarantees of the Exited event in such a case
                    throw new InvalidOperationException("Could not start process: " + process);
                }

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                return await Task.Run(
                           () =>
                               {
                                   process.WaitForExit();
                                   return process.ExitCode;
                               },
                           token).ConfigureAwait(false);
            }
            finally
            {
                RepoLimiterSemaphore.Release();
            }
        }
    }
}