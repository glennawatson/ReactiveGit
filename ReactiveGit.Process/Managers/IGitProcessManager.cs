namespace ReactiveGit.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents a process running GIT.
    /// </summary>
    public interface IGitProcessManager
    {
        /// <summary>
        /// Gets the path to the repository.
        /// </summary>
        string RepositoryPath { get; }

        /// <summary>
        /// Runs a new GIT instance.
        /// </summary>
        /// <param name="gitArguments">The arguments to pass to GIT.</param>
        /// <param name="extraEnvironmentVariables">Environment variables to pass.</param>
        /// <param name="callerMemberName">The caller of the process.</param>
        /// <param name="includeStandardArguments">Include standard git arguments to make it work nicer with this tool.</param>
        /// <returns>A task which will return the exit code from GIT.</returns>
        IObservable<string> RunGit(IEnumerable<string> gitArguments, IDictionary<string, string> extraEnvironmentVariables = null, [CallerMemberName] string callerMemberName = null, bool includeStandardArguments = true);
    }
}