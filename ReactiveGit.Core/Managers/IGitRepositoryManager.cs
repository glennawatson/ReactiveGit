// <copyright file="IGitRepositoryManager.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Core.Managers
{
    using System;
    using System.Reactive;

    /// <summary>
    /// A git repository instance.
    /// </summary>
    public interface IGitRepositoryManager
    {
        /// <summary>
        /// Gets a observable which will monitor the output from GIT.
        /// </summary>
        IObservable<string> GitOutput { get; }

        /// <summary>
        /// Gets a obseravble which will indicate when the repository is updated.
        /// </summary>
        IObservable<Unit> GitUpdated { get; }

        /// <summary>
        /// Gets the path to the repository.
        /// </summary>
        string RepositoryPath { get; }
    }
}