// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

using ReactiveUI;

namespace ReactiveGit.Gui.Core.ViewModel.Content
{
    /// <summary>
    /// View model for view models that contain content.
    /// </summary>
    public abstract class ContentViewModelBase : ReactiveObject, IContentViewModelBase
    {
        private readonly Lazy<string> _contentId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentViewModelBase" /> class.
        /// </summary>
        protected ContentViewModelBase()
        {
            _contentId = new Lazy<string>(() => GetType().Name);
        }

        /// <inheritdoc />
        public string ContentId => _contentId.Value;

        /// <inheritdoc />
        public abstract string FriendlyName { get; }
    }
}