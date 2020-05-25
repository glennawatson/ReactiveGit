// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Linq;
using ReactiveGit.Library.Core.Model;

namespace ReactiveGit.Gui.Core.Model.Branches
{
    /// <summary>
    /// A leaf in the branch.
    /// </summary>
    public class BranchLeaf : BranchNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BranchLeaf" /> class.
        /// </summary>
        /// <param name="branch">The branch that the lead represents.</param>
        public BranchLeaf(GitBranch branch)
            : base(branch.FriendlyName.Split('/').Last(), branch.FriendlyName)
        {
            Branch = branch;
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