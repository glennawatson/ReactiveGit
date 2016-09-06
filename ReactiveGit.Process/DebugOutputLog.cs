namespace Git.VisualStudio
{
    using System.Diagnostics;

    /// <summary>
    /// A output logger that will go to the debug output.
    /// </summary>
    public class DebugOutputLog : IOutputLogger
    {
        /// <inheritdoc />
        public void WriteLine(string text)
        {
            Debug.WriteLine(text);
        }
    }
}
