namespace ReactiveGit.Gui.Base.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using ReactiveGit.Model;

    /// <summary>
    /// View model dealing with git objects.
    /// </summary>
    public interface IGitObjectViewModel : IChildRepositoryDocumentViewModel
    {
        /// <summary>
        /// Gets a command which will reset hard to the specified ref log.
        /// </summary>
        ICommand ResetHard { get; }

        /// <summary>
        /// Gets a command which will reset soft to the specified ref log.
        /// </summary>
        ICommand ResetSoft { get; }

        /// <summary>
        /// Gets a command which will reset soft to the specified ref log.
        /// </summary>
        ICommand ResetMixed { get; }

        /// <summary>
        /// Gets or sets the selected git object.
        /// </summary>
        IGitIdObject SelectedGitObject { get; set; }
    }
}
