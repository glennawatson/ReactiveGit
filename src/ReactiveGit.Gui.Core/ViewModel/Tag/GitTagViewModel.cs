// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using ReactiveGit.Library.Core.Model;
using ReactiveUI;

namespace ReactiveGit.Gui.Core.ViewModel.Tag
{
    /// <summary>
    /// ViewModel for an individual GitTag.
    /// </summary>
    public class GitTagViewModel : ReactiveObject, IGitIdObject
    {
        private readonly GitTag _tag;
        private readonly ObservableAsPropertyHelper<string> _message;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitTagViewModel"/> class.
        /// </summary>
        /// <param name="tag">The target GitTag to Wrap.</param>
        public GitTagViewModel(GitTag tag)
        {
            _tag = tag ?? throw new ArgumentException($"{nameof(tag)} must not be null.", nameof(tag));
            _message = tag.Message.ToProperty(this, x => x.Message, scheduler: RxApp.MainThreadScheduler);
        }

        /// <inheritdoc />
        public string Sha => _tag.Sha;

        /// <inheritdoc />
        public string ShaShort => _tag.ShaShort;

        /// <summary>
        /// Gets the date time the tag was created.
        /// </summary>
        public DateTime DateTime => _tag.DateTime;

        /// <summary>
        /// Gets the name of the tag.
        /// </summary>
        public string Name => _tag.Name;

        /// <summary>
        /// Gets a message about the tag.
        /// </summary>
        public string Message => _message.Value;
    }
}