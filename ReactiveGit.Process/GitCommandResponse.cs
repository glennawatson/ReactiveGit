namespace Git.VisualStudio
{
    /// <summary>
    /// A response after we have initiated a command to GIT.
    /// </summary>
    public class GitCommandResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitCommandResponse"/> class.
        /// </summary>
        /// <param name="success">If the command was successful.</param>
        /// <param name="outputMessage">The command output.</param>
        /// <param name="processOutput">The output from the process.</param>
        /// <param name="returnCode">The return code from the process.</param>
        public GitCommandResponse(bool success, string outputMessage, string processOutput, int returnCode)
        {
            this.Success = success;
            this.OutputMessage = outputMessage;
            this.ProcessOutput = processOutput;
            this.ReturnCode = returnCode;
        }

        /// <summary>
        /// Gets a value indicating whether the GIT command was successful.
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// Gets the output from the command.
        /// </summary>
        public string OutputMessage { get; private set; }

        /// <summary>
        /// Gets the output from the process.
        /// </summary>
        public string ProcessOutput { get; private set; }

        /// <summary>
        /// Gets the return code from the process.
        /// </summary>
        public int ReturnCode { get; private set; }
    }
}
