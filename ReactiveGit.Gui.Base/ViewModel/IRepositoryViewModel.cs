namespace ReactiveGit.Gui.Base.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using ReactiveGit.Model;

    using ReactiveUI;
    using ReactiveUI.Legacy;

    /// <summary>
    /// A view model for a GIT repository.
    /// </summary>
    public interface IRepositoryViewModel
    {
        /// <summary>
        /// Gets a user friendly name of the repository.
        /// </summary>
        string FriendlyName { get; }

        /// <summary>
        /// Gets the path to the repository.
        /// </summary>
        string RepositoryPath { get; }

        /// <summary>
        /// Gets a collection of commit history.
        /// </summary>
        IReadOnlyCollection<GitCommit> CommitHistory { get; }

        /// <summary>
        /// Gets the current branch that the repository is in.
        /// </summary>
        GitBranch CurrentBranch { get; }

        /// <summary>
        /// Gets a command which will change the current branch.
        /// </summary>
        ICommand ChangeBranch { get; }

        /// <summary>
        /// Gets a list of branches on the repository.
        /// </summary>
        IReadOnlyList<GitBranch> Branches { get; }

        /// <summary>
        /// Gets a command that refreshes the values from GIT.
        /// </summary>
        ICommand Refresh { get; }
    }
}
