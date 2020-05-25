// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace ReactiveGit.Gui.Core.ViewModel.Content
{
    /// <summary>
    /// A view model that is used as content in the view layer.
    /// </summary>
    public interface IContentViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the content id of the content.
        /// </summary>
        string ContentId { get; }

        /// <summary>
        /// Gets the name of the document view model.
        /// </summary>
        string FriendlyName { get; }
    }
}