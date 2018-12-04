// <copyright file="IBranchViewModel.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.Core.ViewModel.Branches
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using ReactiveGit.Gui.Core.Model.Branches;

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