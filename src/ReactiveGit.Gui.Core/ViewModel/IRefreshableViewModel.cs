// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows.Input;

namespace ReactiveGit.Gui.Core.ViewModel
{
    /// <summary>
    /// A view model that can be refreshed.
    /// </summary>
    public interface IRefreshableViewModel : IViewModel
    {
        /// <summary>
        /// Gets a command which will refresh the child repository document.
        /// </summary>
        ICommand Refresh { get; }
    }
}