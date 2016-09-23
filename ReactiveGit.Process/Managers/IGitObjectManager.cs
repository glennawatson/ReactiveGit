namespace ReactiveGit.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;
    using System.Text;
    using System.Threading.Tasks;

    using ReactiveGit.Model;

    /// <summary>
    /// Handles operations with Git Objects.
    /// </summary>
    public interface IGitObjectManager
    {
        /// <summary>
        /// Resets the git object.
        /// </summary>
        /// <param name="gitObject">The git object to reset.</param>
        /// <param name="resetMode">The reset mode.</param>
        /// <returns>An observable of the operation.</returns>
        IObservable<Unit> Reset(IGitIdObject gitObject, ResetMode resetMode);
    }
}
