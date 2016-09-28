namespace ReactiveGit.Gui.Core.Model
{
    using System.ComponentModel;

    using ReactiveGit.Core.Managers;
    using ReactiveGit.Core.Model;

    /// <summary>
    /// Details about the repository.
    /// </summary>
    public interface IRepositoryDetails : INotifyPropertyChanged
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
        /// Gets the repository manager.
        /// </summary>
        IGitRepositoryManager RepositoryManager { get; }

        /// <summary>
        /// Gets or sets the selected branch.
        /// </summary>
        GitBranch SelectedBranch { get; set; }
    }
}