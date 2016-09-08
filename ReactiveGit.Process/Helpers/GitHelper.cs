// <copyright file="GitHelper.cs" company="Glenn Watson">
// Copyright (c) Glenn Watson. All rights reserved.
// </copyright>

namespace ReactiveGit.Helpers
{
    using System;
    using System.IO;
    using System.Linq;

    using Microsoft.Win32;

    /// <summary>
    /// Helper class for finding details about the GIT installation.
    /// </summary>
    public class GitHelper
    {
        /// <summary>
        /// Gets the binary path to the GIT executable.
        /// </summary>
        /// <returns>The path to the GIT executable.</returns>
        public static string GetGitBinPath()
        {
            string installationPath = GetGitInstallationPath();
            if (installationPath == null)
            {
                return null;
            }

            string binPath = Path.Combine(installationPath, "usr/bin");
            return Directory.Exists(binPath) ? binPath : Path.Combine(installationPath, "bin");
        }

        /// <summary>
        /// Gets the path to the installation path.
        /// </summary>
        /// <returns>The installation path.</returns>
        public static string GetGitInstallationPath()
        {
            string gitPath = GetInstallPathFromEnvironmentVariable();
            if (gitPath != null)
            {
                return gitPath;
            }

            gitPath = GetInstallPathFromRegistry();
            if (gitPath != null)
            {
                return gitPath;
            }

            gitPath = GetInstallPathFromProgramFiles();
            return gitPath;
        }

        /// <summary>
        /// Attempt to get the installation path from the path variable.
        /// </summary>
        /// <returns>The installation path or null if unable to be found.</returns>
        public static string GetInstallPathFromEnvironmentVariable()
        {
            string path = Environment.GetEnvironmentVariable("PATH");
            if (path == null)
            {
                return null;
            }

            string[] allPaths = path.Split(';');
            string gitPath = allPaths.FirstOrDefault(p => p.ToLower().TrimEnd('\\').EndsWith("git\\cmd"));
            if (gitPath != null && Directory.Exists(gitPath))
            {
                gitPath = Directory.GetParent(gitPath).FullName.TrimEnd('\\');
            }

            return gitPath;
        }

        /// <summary>
        /// Attempt to get the installation path from the registry.
        /// </summary>
        /// <returns>The installation path or null if unable to be found.</returns>
        public static string GetInstallPathFromRegistry()
        {
            // Check reg key for msysGit 2.6.1+
            object installLocation = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\GitForWindows", "InstallPath", null);
            if (installLocation != null && Directory.Exists(installLocation.ToString().TrimEnd('\\')))
            {
                return installLocation.ToString().TrimEnd('\\');
            }

            // Check uninstall key for older versions
            installLocation = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Git_is1", "InstallLocation", null);
            if (installLocation != null && Directory.Exists(installLocation.ToString().TrimEnd('\\')))
            {
                return installLocation.ToString().TrimEnd('\\');
            }

            // try 32-bit OS
            installLocation = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Git_is1", "InstallLocation", null);
            if (installLocation != null && Directory.Exists(installLocation.ToString().TrimEnd('\\')))
            {
                return installLocation.ToString().TrimEnd('\\');
            }

            return null;
        }

        /// <summary>
        /// Attempts to get the installation path searching the program files directory.
        /// </summary>
        /// <returns>The installation path or null if unable to be found.</returns>
        public static string GetInstallPathFromProgramFiles()
        {
            // If this is a 64bit OS, and the user installed 64bit git, then explictly search that folder.
            if (Environment.Is64BitOperatingSystem)
            {
                object x64ProgramFiles = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion", "ProgramW6432Dir", null);
                if (x64ProgramFiles != null)
                {
                    string gitPathX64 = Path.Combine(x64ProgramFiles.ToString(), "git");
                    if (Directory.Exists(gitPathX64))
                    {
                        return gitPathX64.TrimEnd('\\');
                    }
                }
            }

            // Else, this is a 64bit or a 32bit machine, and the user installed 32bit git
            string gitPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "git");
            return Directory.Exists(gitPath) ? gitPath.TrimEnd('\\') : null;
        }
    }
}