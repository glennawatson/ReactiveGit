// <copyright file="ITagViewModel.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.Core.ViewModel.Tag
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using ReactiveGit.Core.Model;
    using ReactiveGit.Gui.Core.ViewModel.GitObject;

    /// <summary>
    /// View model for handling tags.
    /// </summary>
    public interface ITagViewModel : IGitObjectViewModel, ISupportViewModel
    {
        /// <summary>
        /// Gets the tags contained within the repository.
        /// </summary>
        IEnumerable<GitTag> Tags { get; }

        /// <summary>
        /// Gets the selected tag.
        /// </summary>
        GitTag SelectedTag { get; }
    }
}
