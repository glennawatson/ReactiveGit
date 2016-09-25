namespace ReactiveGit.Core.ExtensionMethods
{
    using System;

    /// <summary>
    /// Extension methods related to strings.
    /// </summary>
    public static class StringExtensionMethods
    {
        /// <summary>
        /// Separates a string into an array of the lines within the string.
        /// </summary>
        /// <param name="str">The string to convert into an array.</param>
        /// <returns>The array of lines.</returns>
        public static string[] ToArrayOfLines(this string str)
        {
            if (str == null)
            {
                return new string[0];
            }

            return str.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Trims any empty lines out of the text.
        /// </summary>
        /// <param name="input">The string with potential empty lines.</param>
        /// <returns>The string minus any new lines.</returns>
        public static string TrimEmptyLines(this string input)
        {
            input = input.Trim('\r', '\n');
            input = input.Trim();

            return input;
        }
    }
}