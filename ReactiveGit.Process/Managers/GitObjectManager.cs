namespace ReactiveGit.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;
    using System.Text;
    using System.Threading.Tasks;

    using ReactiveGit.ExtensionMethods;
    using ReactiveGit.Model;

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
