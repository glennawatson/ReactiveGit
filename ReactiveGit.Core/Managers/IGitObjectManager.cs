namespace ReactiveGit.Core.Managers
{
    using System;
    using System.Reactive;

    using ReactiveGit.Core.Model;

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