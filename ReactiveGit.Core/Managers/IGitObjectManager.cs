// <copyright file="IGitObjectManager.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Core.Managers
{
    using System;
    using System.Reactive;
    using System.Reactive.Concurrency;

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
        /// <param name="scheduler">The scheduler to schedule it on. By default a TaskPool Scheduler.</param>
        /// <returns>An observable of the operation.</returns>
        IObservable<Unit> Reset(IGitIdObject gitObject, ResetMode resetMode, IScheduler scheduler = null);

        /// <summary>
        /// Checks out the selected git object.
        /// </summary>
        /// <param name="gitObject">The git object to check out.</param>
        /// <param name="force">If we should force the operation or not.</param>
        /// <param name="scheduler">The scheduler to schedule it on. By default a TaskPool Scheduler.</param>
        /// <returns>An observable of the operation.</returns>
        IObservable<Unit> Checkout(IGitIdObject gitObject, bool force, IScheduler scheduler = null);
    }
}