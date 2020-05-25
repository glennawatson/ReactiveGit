// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveGit.Gui.Core.ViewModel.GitObject;
using ReactiveGit.Library.Core.Model;

namespace ReactiveGit.Gui.Core.ViewModel.Tag
{
    /// <summary>
    /// View model for handling tags.
    /// </summary>
    public interface ITagsViewModel : IGitObjectViewModel, ISupportViewModel
    {
        /// <summary>
        /// Gets the tags contained within the repository.
        /// </summary>
        IEnumerable<GitTagViewModel> Tags { get; }

        /// <summary>
        /// Gets the selected tag.
        /// </summary>
        GitTagViewModel SelectedTag { get; }
    }
}
