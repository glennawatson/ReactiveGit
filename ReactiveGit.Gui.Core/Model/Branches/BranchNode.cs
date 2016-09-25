namespace ReactiveGit.Gui.Core.Model.Branches
{
    using System;

    /// <summary>
    /// A node in the branch structure.
    /// </summary>
    public abstract class BranchNode : IEquatable<BranchNode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BranchNode"/> class.
        /// </summary>
        /// <param name="name">The name of the node.</param>
        /// <param name="fullName">The full name of the node.</param>
        protected BranchNode(string name, string fullName)
        {
            this.Name = name;
            this.FullName = fullName;
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

            return string.Equals(this.FullName, other.FullName);
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
            return other != null && this.Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return this.FullName?.GetHashCode() ?? 0;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.Name;
        }
    }
}