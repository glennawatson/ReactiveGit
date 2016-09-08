namespace ReactiveGit.Loggers
{
    /// <summary>
    /// Represents the ability to output to the end user.
    /// </summary>
    public interface IOutputLogger
    {
        /// <summary>
        /// Writes a line of text to the logger.
        /// </summary>
        /// <param name="text">The text to output.</param>
        void WriteLine(string text);
    }
}