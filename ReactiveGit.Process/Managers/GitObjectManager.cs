namespace ReactiveGit.Process.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Reactive;
    using System.Reactive.Concurrency;

    using ReactiveGit.Core.ExtensionMethods;
    using ReactiveGit.Core.Managers;
    using ReactiveGit.Core.Model;

    /// <summary>
    /// Responsible for handling operations in regards to git objects.
    /// </summary>
    public class GitObjectManager : IGitObjectManager
    {
        private readonly IGitProcessManager gitProcessManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitObjectManager" /> class.
        /// </summary>
        /// <param name="gitProcessManager">The git process to use.</param>
        public GitObjectManager(IGitProcessManager gitProcessManager)
        {
            if (gitProcessManager == null)
            {
                throw new ArgumentNullException(nameof(gitProcessManager));
            }

            this.gitProcessManager = gitProcessManager;
        }

        /// <inheritdoc />
        public IObservable<Unit> Reset(IGitIdObject gitObject, ResetMode resetMode, IScheduler scheduler = null)
        {
            string[] arguments = { "reset", $"--{resetMode.ToString().ToLowerInvariant()}", gitObject.Sha };

            return this.gitProcessManager.RunGit(arguments, scheduler: scheduler).WhenDone();
        }

        /// <inheritdoc />
        public IObservable<Unit> Checkout(IGitIdObject gitObject, bool force, IScheduler scheduler = null)
        {
            IList<string> arguments = new List<string>();

            arguments.Add("checkout");

            if (force)
            {
                arguments.Add("--force");
            }

            arguments.Add(gitObject.Sha);

            return this.gitProcessManager.RunGit(arguments, scheduler: scheduler).WhenDone();
        }
    }
}