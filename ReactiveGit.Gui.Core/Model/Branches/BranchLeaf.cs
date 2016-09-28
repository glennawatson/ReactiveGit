namespace ReactiveGit.Gui.Core.Model.Branches
{
    using System.Linq;

    using ReactiveGit.Core.Model;

    /// <summary>
    /// A leaf in the branch.
    /// </summary>
    public class BranchLeaf : BranchNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BranchLeaf"/> class.
        /// </summary>
        /// <param name="branch">The branch that the lead represents.</param>
        public BranchLeaf(GitBranch branch)
            : base(branch.FriendlyName.Split('/').Last(), branch.FriendlyName)
        {
            this.Branch = branch;
        }

        /// <summary>
        /// Gets the branch that is represented by the leaf.
        /// </summary>
        public GitBranch Branch { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the node is checked out.
        /// </summary>
        public bool IsCheckedOut { get; set; }
    }
}