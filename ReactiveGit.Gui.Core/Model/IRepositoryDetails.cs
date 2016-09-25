namespace ReactiveGit.Gui.Core.Model
{
    using ReactiveGit.Core.Managers;

    /// <summary>
    /// Details about the repository.
    /// </summary>
    public interface IRepositoryDetails
    {
        /// <summary>
        /// Gets the branch manager.
        /// </summary>
        IBranchManager BranchManager { get; }

        /// <summary>
        /// Gets the friendly name of the repository.
        /// </summary>
        string FriendlyName { get; }

        /// <summary>
        /// Gets the git object manager.
        /// </summary>
        IGitObjectManager GitObjectManager { get; }

        /// <summary>
        /// Gets the rebase manager.
        /// </summary>
        IRebaseManager RebaseManager { get; }

        /// <summary>
        /// Gets the ref log manager.
        /// </summary>
        IRefLogManager RefLogManager { get; }

        /// <summary>
        /// Gets the path to the repository.
        /// </summary>
        string RepositoryPath { get; }
    }
}