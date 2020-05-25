// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ReactiveGit.Gui.Core.ViewModel.GitObject
{
    /// <summary>
    /// A informational git object action that performs no actions.
    /// Usually used as a parent.
    /// </summary>
    public class InfoGitObjectAction : IGitObjectAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InfoGitObjectAction"/> class.
        /// </summary>
        /// <param name="name">The name of the action.</param>
        /// <param name="childActions">The child items.</param>
        public InfoGitObjectAction(string name, IReadOnlyList<IGitObjectAction> childActions)
        {
            Name = name;
            ExecuteAction = null;
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
