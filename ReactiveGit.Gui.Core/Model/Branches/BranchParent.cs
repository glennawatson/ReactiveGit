namespace ReactiveGit.Gui.Core.Model.Branches
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents a parent in the branch node system.
    /// </summary>
    public class BranchParent : BranchNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BranchParent"/> class.
        /// </summary>
        /// <param name="name">The name of the parent.</param>
        /// <param name="fullName">The full name of the parent.</param>
        public BranchParent(string name, string fullName)
            : base(name, fullName)
        {
            this.ChildNodes = new ObservableCollection<BranchNode>();
        }

        /// <summary>
        /// Gets a read only list of child nodes.
        /// </summary>
        public IList<BranchNode> ChildNodes { get; }
    }
}