// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive;
using System.Windows.Input;

using ReactiveGit.Library.Core.Model;

using ReactiveUI;

namespace ReactiveGit.Gui.Core.ViewModel.GitObject
{
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
            ExecuteAction = command;
            Name = name;
            ChildActions = childActions;
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public ICommand ExecuteAction { get; }

        /// <inheritdoc />
        public IReadOnlyList<IGitObjectAction> ChildActions { get; }
    }
}
