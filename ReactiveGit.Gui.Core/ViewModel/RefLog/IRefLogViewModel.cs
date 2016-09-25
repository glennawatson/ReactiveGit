namespace ReactiveGit.Gui.Core.ViewModel.RefLog
{
    using System.Collections.Generic;

    using ReactiveGit.Core.Model;

    /// <summary>
    /// View model for handling ref log.
    /// </summary>
    public interface IRefLogViewModel : IGitObjectViewModel, ISupportViewModel
    {
        /// <summary>
        /// Gets a collection of ref log entries for the current branch.
        /// </summary>
        IEnumerable<GitRefLog> RefLog { get; }
    }
}