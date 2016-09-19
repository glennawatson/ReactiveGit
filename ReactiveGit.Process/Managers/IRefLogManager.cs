namespace ReactiveGit.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using ReactiveGit.Model;

    /// <summary>
    /// Represents getting items out of the ref log.
    /// </summary>
    public interface IRefLogManager
    {
        /// <summary>
        /// Gets the ref log for the desired branch. If none is specified then it's all branches.
        /// </summary>
        /// <param name="branch">The branch to get the ref log for.</param>
        /// <returns>The ref log items.</returns>
        IObservable<GitRefLog> GetRefLog(GitBranch branch);
    }
}
