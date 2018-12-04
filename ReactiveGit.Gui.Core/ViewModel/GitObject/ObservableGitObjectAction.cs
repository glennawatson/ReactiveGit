// <copyright file="ObservableGitObjectAction.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.Core.ViewModel.GitObject
{
    using System;
    using System.Collections.Generic;
    using System.Reactive;
    using System.Windows.Input;

    using ReactiveGit.Core.Model;

    using ReactiveUI;

    /// <summary>
    /// A action that will perform a observable.
    /// </summary>
    public class ObservableGitObjectAction : ReactiveObject, IGitObjectAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableGitObjectAction"/> class.
        /// </summary>
        /// <param name="name">The name of the action.</param>
        /// <param name="command">The command to perform.</param>
        /// <param name="childActions">A optional list of child items.</param>
        public ObservableGitObjectAction(string name, ReactiveCommand<Unit, Unit> command,  IReadOnlyList<IGitObjectAction> childActions = null)
        {
            this.ExecuteAction = command;
            this.Name = name;
            this.ChildActions = childActions;
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public ICommand ExecuteAction { get; }

        /// <inheritdoc />
        public IReadOnlyList<IGitObjectAction> ChildActions { get; }
    }
}
