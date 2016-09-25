namespace ReactiveGit.Process.Managers
{
    using System;
    using System.Reactive;

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
        /// Initializes a new instance of the <see cref="GitObjectManager"/> class.
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
        public IObservable<Unit> Reset(IGitIdObject gitObject, ResetMode resetMode)
        {
            string[] arguments = { "reset", $"--{resetMode.ToString().ToLowerInvariant()}", gitObject.Sha };

            return this.gitProcessManager.RunGit(arguments).WhenDone();
        }
    }
}
