// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Windows.Input;

using ReactiveGit.Gui.Core.Model.Branches;

namespace ReactiveGit.Gui.Core.ViewModel.Branches
{
    /// <summary>
    /// View model associated with branches.
    /// </summary>
    public interface IBranchViewModel : ISupportViewModel, IRefreshableViewModel
    {
        /// <summary>
        /// Gets a list of branches on the repository.
        /// </summary>
        IEnumerable<BranchNode> Branches { get; }

        /// <summary>
        /// Gets a command which will change the current branch.
        /// </summary>
        ICommand CheckoutBranch { get; }
    }
}