// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows.Input;

namespace ReactiveGit.Gui.Core.ExtensionMethods
{
    /// <summary>
    /// Extension methods relating to the ICommand interface.
    /// </summary>
    public static class CommandExtensionMethods
    {
        /// <summary>
        /// Invokes a command. First checks if it can execute.
        /// </summary>
        /// <param name="command">The command to execute if possible.</param>
        public static void InvokeCommand(this ICommand command)
        {
            if (command.CanExecute(null))
            {
                command.Execute(null);
            }
        }
    }
}