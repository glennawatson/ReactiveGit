namespace ReactiveGit.Gui.Core.ViewModel.GitObject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;

    /// <summary>
    /// A action that can be performed on a GIT object.
    /// </summary>
    public interface IGitObjectAction
    {
        /// <summary>
        /// Gets the name of the action.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a command which will execute the GIT action.
        /// </summary>
        ICommand ExecuteAction { get; }

        /// <summary>
        /// Gets a collection of child actions.
        /// </summary>
        IReadOnlyList<IGitObjectAction> ChildActions { get; }
    }
}
