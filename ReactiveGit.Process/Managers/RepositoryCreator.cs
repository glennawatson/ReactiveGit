// <copyright file="RepositoryCreator.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Process.Managers
{
    using System;
    using System.IO;
    using System.Reactive;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;

    using ReactiveGit.Core.Exceptions;
    using ReactiveGit.Core.Managers;
    using ReactiveGit.Process.Helpers;

    /// <summary>
    /// Creates a new repository in a directory if the directory is empty.
    /// </summary>
    public class RepositoryCreator : IRepositoryCreator
    {
        private readonly Func<string, IGitProcessManager> processManagerFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryCreator" /> class.
        /// </summary>
        /// <param name="processManagerFunc">Function to creating a git process.</param>
        public RepositoryCreator(Func<string, IGitProcessManager> processManagerFunc)
        {
            this.processManagerFunc = processManagerFunc;
        }

        /// <summary>
        /// Creates a repository.
        /// </summary>
        /// <param name="directoryPath">The path to the new repository.</param>
        /// <param name="scheduler">The scheduler to use when creating the repository.</param>
        /// <returns>An observable monitoring the action.</returns>
        public IObservable<Unit> CreateRepository(string directoryPath, IScheduler scheduler = null)
        {
            return Observable.Create<Unit>(
                observer =>
                    {
                        if (Directory.Exists(directoryPath) == false)
                        {
                            throw new GitProcessException("Cannot find directory");
                        }

                        if (FileHelper.IsDirectoryEmpty(directoryPath) == false)
                        {
                            throw new GitProcessException("The directory is not empty.");
                        }

                        IGitProcessManager gitProcess = this.processManagerFunc(directoryPath);
                        IDisposable disposable = gitProcess.RunGit(new[] { "init" }, showInOutput: true, scheduler: scheduler).Subscribe(
                            _ => { },
                            observer.OnError,
                            () =>
                                {
                                    observer.OnNext(Unit.Default);
                                    observer.OnCompleted();
                                });

                        return disposable;
                    });
        }
    }
}