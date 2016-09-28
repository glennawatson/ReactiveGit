namespace ReactiveGit.Gui.Core.ViewModel.Output
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;

    /// <summary>
    /// Represents the window for output from GIT.
    /// </summary>
    public interface IOutputViewModel : ISupportViewModel
    {
        /// <summary>
        /// Gets the output from GIT.
        /// </summary>
        string Output { get; }

        /// <summary>
        /// Gets a command which will clear the output.
        /// </summary>
        ICommand Clear { get; }
    }
}
