namespace ReactiveGit.Gui.Core.ViewModel.Branches
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using ReactiveGit.Gui.Core.Model.Branches;

    /// <summary>
    /// View model associated with branches.
    /// </summary>
    public interface IBranchViewModel : ISupportViewModel
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