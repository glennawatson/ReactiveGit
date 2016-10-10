namespace ReactiveGit.Gui.Core.ViewModel.GitObject
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using ReactiveGit.Core.Model;

    using ReactiveUI;

    /// <summary>
    /// View model dealing with GIT objects.
    /// </summary>
    public interface IGitObjectViewModel : IRefreshableViewModel
    {
        /// <summary>
        /// Gets a collection of actions that can be performed on the git object.
        /// </summary>
        IReadOnlyList<IGitObjectAction> Actions { get; }

            /// <summary>
        /// Gets or sets the selected GIT object.
        /// </summary>
        IGitIdObject SelectedGitObject { get; set; }
    }
}