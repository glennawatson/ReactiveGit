namespace ReactiveGit.Managers
{
    using System;
    using System.Reactive;

    /// <summary>
    /// Responsible for creating a repository.
    /// </summary>
    public interface IRepositoryCreator
    {
        /// <summary>
        /// Creates a repository.
        /// </summary>
        /// <param name="directoryPath">The path to the new repository.</param>
        /// <returns>An observable monitoring the action.</returns>
        IObservable<Unit> CreateRepository(string directoryPath);
    }
}