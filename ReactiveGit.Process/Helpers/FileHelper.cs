namespace ReactiveGit.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Helper which assists with file paths.
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Determine if the directory is empty, ie. no files and no sub-directories
        /// </summary>
        /// <param name="path">directory to inspect</param>
        /// <returns>true if directory is empty, false otherwise</returns>
        public static bool IsDirectoryEmpty(string path)
        {
            return IsDirectoryEmpty(new DirectoryInfo(path));
        }

        /// <summary>
        /// Determine if the directory is empty, ie. no files and no sub-directories
        /// </summary>
        /// <param name="directory">directory to inspect</param>
        /// <returns>true if directory is empty, false otherwise</returns>
        public static bool IsDirectoryEmpty(DirectoryInfo directory)
        {
            FileInfo[] files = directory.GetFiles();
            DirectoryInfo[] subdirs = directory.GetDirectories();

            return files.Length == 0 && subdirs.Length == 0;
        }
    }
}
