// <copyright file="IGitIdObject.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Core.Model
{
    /// <summary>
    /// A git object that is represented by a SHA id.
    /// </summary>
    public interface IGitIdObject
    {
        /// <summary>
        /// Gets the full length SHA id.
        /// </summary>
        string Sha { get; }

        /// <summary>
        /// Gets the shortened abbreviated SHA id.
        /// </summary>
        string ShaShort { get; }
    }
}