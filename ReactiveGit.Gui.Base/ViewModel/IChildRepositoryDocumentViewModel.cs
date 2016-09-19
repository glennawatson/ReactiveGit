namespace ReactiveGit.Gui.Base.ViewModel
{
    using System.Windows.Input;

    using ReactiveGit.Model;

    /// <summary>
    /// A child of the main repository document view model.
    /// </summary>
    public interface IChildRepositoryDocumentViewModel
    {
        /// <summary>
        /// Gets a command which will refresh the child repository document.
        /// </summary>
        ICommand Refresh { get; }

        /// <summary>
        /// Gets or sets the current branch.
        /// </summary>
        GitBranch CurrentBranch { get; set; }
    }
}