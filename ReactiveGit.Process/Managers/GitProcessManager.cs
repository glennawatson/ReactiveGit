namespace ReactiveGit.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reactive.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using ReactiveGit.Exceptions;
    using ReactiveGit.Helpers;
    using ReactiveGit.Loggers;

    /// <summary>
    /// Manages and starts GIT processes.
    /// </summary>
    public class GitProcessManager : IGitProcessManager
    {
        private static readonly SemaphoreSlim RepoLimiterSemaphore = new SemaphoreSlim(1, 1);

        private readonly IOutputLogger outputLogger;

        private readonly string repoDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitProcessManager"/> class.
        /// </summary>
        /// <param name="repoDirectory">The location of the GIT repository.</param>
        /// <param name="newOutputLogger">The output logger where to send output.</param>
        public GitProcessManager(string repoDirectory, IOutputLogger newOutputLogger)
        {
            this.repoDirectory = repoDirectory;
            this.outputLogger = newOutputLogger;
        }

        /// <inheritdoc />
        public string RepositoryPath => this.repoDirectory;

        /// <summary>
        /// Runs a new instance of GIT.
        /// </summary>
        /// <param name="gitArgumentsEnumerable">The arguments to pass to GIT.</param>
        /// <param name="extraEnvironmentVariables">Any environment variables to pass for the process.</param>
        /// <param name="callerMemberName">The member calling the process.</param>
        /// <param name="includeStandardArguments">If to include standard arguments useful in a script environment.</param>
        /// <returns>A task which will return the response from the GIT process.</returns>
        public IObservable<string> RunGit(IEnumerable<string> gitArgumentsEnumerable, IDictionary<string, string> extraEnvironmentVariables = null, [CallerMemberName] string callerMemberName = null, bool includeStandardArguments = true)
        {
            return Observable.Create<string>(async (observer, token) =>
                {
                    var gitArguments = string.Join(" ", gitArgumentsEnumerable);
                    if (includeStandardArguments)
                    {
                        gitArguments = $"--no-pager -c color.branch=false -c color.diff=false -c color.status=false -c diff.mnemonicprefix=false -c core.quotepath=false {gitArguments}";
                    }

                    this.outputLogger?.WriteLine($"execute: git {gitArguments}");

                    using (Process process = CreateGitProcess(gitArguments, this.repoDirectory))
                    {
                        if (extraEnvironmentVariables != null)
                        {
                            foreach (KeyValuePair<string, string> kvp in extraEnvironmentVariables)
                            {
                                process.StartInfo.EnvironmentVariables.Add(kvp.Key, kvp.Value);
                            }
                        }

                        StringBuilder errorOutput = new StringBuilder();
                        process.ErrorDataReceived += (sender, e) =>
                            {
                                if (e.Data == null)
                                {
                                    return;
                                }

                                this.outputLogger?.WriteLine(e.Data);
                                errorOutput.AppendLine(e.Data);
                                observer.OnNext(e.Data);
                            };

                        process.OutputDataReceived += (sender, e) =>
                            {
                                if (e.Data == null)
                                {
                                    return;
                                }

                                this.outputLogger?.WriteLine(e.Data);
                                errorOutput.AppendLine(e.Data);
                                observer.OnNext(e.Data);
                            };

                        if (token.IsCancellationRequested)
                        {
                            observer.OnCompleted();
                            return;
                        }

                        int returnValue = await RunProcessAsync(process, token);

                        if (returnValue != 0)
                        {
                            observer.OnError(new GitProcessException(string.Join(" ", gitArgumentsEnumerable), errorOutput.ToString()));
                        }

                        observer.OnCompleted();
                    }
                });
        }

        private static Process CreateGitProcess(string arguments, string repoDirectory)
        {
            string gitInstallationPath = GitHelper.GetGitInstallationPath();
            string pathToGit = Path.Combine(Path.Combine(gitInstallationPath, @"bin\git.exe"));
            return new Process { StartInfo = { CreateNoWindow = true, UseShellExecute = false, RedirectStandardInput = true, RedirectStandardOutput = true, RedirectStandardError = true, FileName = pathToGit, Arguments = arguments, WorkingDirectory = repoDirectory, StandardErrorEncoding = Encoding.UTF8, StandardOutputEncoding = Encoding.UTF8 }, EnableRaisingEvents = true };
        }

        private static async Task<int> RunProcessAsync(Process process, CancellationToken token)
        {
            await RepoLimiterSemaphore.WaitAsync(token);

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

                return await Task.Run(() =>
                           {
                               process.WaitForExit();
                               return process.ExitCode;
                           }, token);
            }
            finally
            {
                RepoLimiterSemaphore.Release();
            }
        }
    }
}