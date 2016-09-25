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
