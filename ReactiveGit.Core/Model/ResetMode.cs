namespace ReactiveGit.Core.Model
{
    /// <summary>
    /// The reset mode when it comes to resetting a git object.
    /// </summary>
    public enum ResetMode
    {
        /// <summary>
        /// Soft commit will not change the index/working directory, only the head. 
        /// </summary>
        Soft,

        /// <summary>
        /// Changes the head and index, but not the working directory.
        /// </summary>
        Mixed,

        /// <summary>
        /// Resets the head, index and working directory.
        /// </summary>
        Hard,
    }
}
