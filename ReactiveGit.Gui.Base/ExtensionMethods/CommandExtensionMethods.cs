namespace ReactiveGit.Gui.Base.ExtensionMethods
{
    using System.Windows.Input;

    /// <summary>
    /// Extension methods relating to the ICommand interface.
    /// </summary>
    public static class CommandExtensionMethods
    {
        /// <summary>
        /// Invokes a command. First checks if it can execute.
        /// </summary>
        /// <param name="command">The command to execute if possible.</param>
        public static void InvokeCommand(this ICommand command)
        {
            if (command.CanExecute(null))
            {
                command.Execute(null);
            } 
        }
    }
}
