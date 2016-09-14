namespace ReactiveGit.Gui.Base.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;

    /// <summary>
    /// A child of the main repository document view model.
    /// </summary>
    public interface IChildRepositoryDocumentViewModel
    {
        /// <summary>
        /// Gets a command which will refresh the child repository document.
        /// </summary>
        ICommand Refresh { get; }
    }
}
