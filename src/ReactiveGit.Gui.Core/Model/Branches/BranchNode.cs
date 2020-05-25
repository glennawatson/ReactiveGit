// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace ReactiveGit.Gui.Core.Model.Branches
{
    /// <summary>
    /// A node in the branch structure.
    /// </summary>
    public abstract class BranchNode : IEquatable<BranchNode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BranchNode" /> class.
        /// </summary>
        /// <param name="name">The name of the node.</param>
        /// <param name="fullName">The full name of the node.</param>
        protected BranchNode(string name, string fullName)
        {
            Name = name;
            FullName = fullName;
        }

        /// <summary>
        /// Gets gets or sets the full name of the node.
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// Gets the name of the node.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Determines if the left and the right are logically equal.
        /// </summary>
        /// <param name="left">The left to compare.</param>
        /// <param name="right">The right to compare.</param>
        /// <returns>If they are logically equal.</returns>
        public static bool operator ==(BranchNode left, BranchNode right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Determines if the left and the right are not logically equal.
        /// </summary>
        /// <param name="left">The left to compare.</param>
        /// <param name="right">The right to compare.</param>
        /// <returns>If they are not logically equal.</returns>
        public static bool operator !=(BranchNode left, BranchNode right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public bool Equals(BranchNode other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(FullName, other.FullName, StringComparison.InvariantCulture);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var other = obj as BranchNode;
            return (other != null) && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return FullName?.GetHashCode() ?? 0;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Name;
        }
    }
}