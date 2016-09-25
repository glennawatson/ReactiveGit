namespace ReactiveGit.Gui.Core.ViewModel
{
    using System.Windows.Input;

    using ReactiveGit.Core.Model;
    using ReactiveGit.Gui.Core.Model;

    /// <summary>
    /// A child of the main repository document view model.
    /// </summary>
    public interface ISupportViewModel
    {
        /// <summary>
        /// Gets or sets the current branch.
        /// </summary>
        GitBranch CurrentBranch { get; set; }

        /// <summary>
        /// Gets the name of the document view model.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a command which will refresh the child repository document.
        /// </summary>
        ICommand Refresh { get; }

        /// <summary>
        /// Gets or sets the repository details.
        /// </summary>
        IRepositoryDetails RepositoryDetails { get; set; }
    }
}