namespace ReactiveGit.Gui.Base.ViewModel.RefLog
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using ReactiveGit.Model;

    /// <summary>
    /// View model for handling ref log.
    /// </summary>
    public interface IRefLogViewModel : IGitObjectViewModel
    {
        /// <summary>
        /// Gets a collection of ref log entries for the current branch.
        /// </summary>
        IEnumerable<GitRefLog> RefLog { get; }
    }
}