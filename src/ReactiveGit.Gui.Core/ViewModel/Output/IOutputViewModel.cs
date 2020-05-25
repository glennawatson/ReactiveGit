// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows.Input;

namespace ReactiveGit.Gui.Core.ViewModel.Output
{
    /// <summary>
    /// Represents the window for output from GIT.
    /// </summary>
    public interface IOutputViewModel : ISupportViewModel
    {
        /// <summary>
        /// Gets a command which will clear the output.
        /// </summary>
        ICommand Clear { get; }

        /// <summary>
        /// Gets the output from GIT.
        /// </summary>
        string Output { get; }
    }
}