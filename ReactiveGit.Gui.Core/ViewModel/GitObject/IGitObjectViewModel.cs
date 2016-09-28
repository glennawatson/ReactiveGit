namespace ReactiveGit.Gui.Core.ViewModel
{
    using System.Windows.Input;

    using ReactiveGit.Core.Model;

    /// <summary>
    /// View model dealing with GIT objects.
    /// </summary>
    public interface IGitObjectViewModel : IRefreshableViewModel
    {
        /// <summary>
        /// Gets a command which will reset hard to the specified ref log.
        /// </summary>
        ICommand ResetHard { get; }

        /// <summary>
        /// Gets a command which will reset soft to the specified ref log.
        /// </summary>
        ICommand ResetMixed { get; }

        /// <summary>
        /// Gets a command which will reset soft to the specified ref log.
        /// </summary>
        ICommand ResetSoft { get; }

        /// <summary>
        /// Gets or sets the selected GIT object.
        /// </summary>
        IGitIdObject SelectedGitObject { get; set; }
    }
}