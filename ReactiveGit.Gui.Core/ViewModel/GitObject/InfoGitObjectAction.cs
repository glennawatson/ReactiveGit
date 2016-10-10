namespace ReactiveGit.Gui.Core.ViewModel.GitObject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;

    /// <summary>
    /// A informational git object action that performs no actions.
    /// Usually used as a parent.
    /// </summary>
    public class InfoGitObjectAction : IGitObjectAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InfoGitObjectAction"/> class.
        /// </summary>
        /// <param name="name">The name of the action.</param>
        /// <param name="childActions">The child items.</param>
        public InfoGitObjectAction(string name, IReadOnlyList<IGitObjectAction> childActions)
        {
            this.Name = name;
            this.ExecuteAction = null;
            this.ChildActions = childActions;
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public ICommand ExecuteAction { get; }

        /// <inheritdoc />
        public IReadOnlyList<IGitObjectAction> ChildActions { get; }
    }
}
