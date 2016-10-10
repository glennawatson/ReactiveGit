namespace ReactiveGit.Core.Managers
{
    using System;
    using System.Reactive;
    using System.Reactive.Concurrency;

    /// <summary>
    /// Responsible for creating a repository.
    /// </summary>
    public interface IRepositoryCreator
    {
        /// <summary>
        /// Creates a repository.
        /// </summary>
        /// <param name="directoryPath">The path to the new repository.</param>
        /// <param name="scheduler">The scheduler to schedule the task on.</param>
        /// <returns>An observable monitoring the action.</returns>
        IObservable<Unit> CreateRepository(string directoryPath, IScheduler scheduler = null);
    }
}