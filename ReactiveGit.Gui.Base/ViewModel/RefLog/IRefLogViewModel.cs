namespace ReactiveGit.Gui.Base.ViewModel.RefLog
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using ReactiveGit.Model;

    /// <summary>
    /// View model for handling ref log.
    /// </summary>
    public interface IRefLogViewModel : IChildRepositoryDocumentViewModel
    {
        /// <summary>
        /// Gets a collection of ref log entries for the current branch.
        /// </summary>
        IEnumerable<GitRefLog> RefLog { get; }

        /// <summary>
        /// Gets a command which will reset hard to the specified ref log.
        /// </summary>
        ICommand ResetHardRefLog { get; }

        /// <summary>
        /// Gets a command which will reset soft to the specified ref log.
        /// </summary>
        ICommand ResetSoftRefLog { get; }
    }
}