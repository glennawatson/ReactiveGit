// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace ReactiveGit.Gui.WPF
{
    [SuppressUnmanagedCodeSecurity]
    internal static class SafeNativeMethods
    {
        [DllImport("user32.dll")]
        internal static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref SafeNativeMethods.WindowPlacement lpwndpl);

        [DllImport("user32.dll")]
        internal static extern bool GetWindowPlacement(IntPtr hWnd, out SafeNativeMethods.WindowPlacement lpwndpl);

        /// <summary>
        /// Struct that is required by the window placement structure but is not used.
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct Rectangle
        {
            /// <summary>
            /// The left location of the window.
            /// </summary>
            public int Left;

            /// <summary>
            /// The top location of the window.
            /// </summary>
            public int Top;

            /// <summary>
            /// The right location of the window.
            /// </summary>
            public int Right;

            /// <summary>
            /// The bottom of the window.
            /// </summary>
            public int Bottom;

            /// <summary>
            /// Initializes a new instance of the <see cref="ReactiveGit.Gui.WPF.SafeNativeMethods.Rectangle"/> struct.
            /// </summary>
            /// <param name="left">The left of the window.</param>
            /// <param name="top">The top of the window.</param>
            /// <param name="right">The right of the window.</param>
            /// <param name="bottom">The bottom of the window.</param>
            public Rectangle(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }
        }

        /// <summary>
        /// A structure required by the window placement structure but is not used.
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            /// <summary>
            /// The location in the X coordinate.
            /// </summary>
            public int X;

            /// <summary>
            /// The location in the Y coordinate.
            /// </summary>
            public int Y;

            /// <summary>
            /// Initializes a new instance of the <see cref="ReactiveGit.Gui.WPF.SafeNativeMethods.Point"/> struct.
            /// </summary>
            /// <param name="x">The x coordinate.</param>
            /// <param name="y">The y coordinate.</param>
            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        /// <summary>
        /// Stores the position, size and state of a window.
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct WindowPlacement
        {
            /// <summary>
            /// Gets the length of the window.
            /// </summary>
            public int Length;

            /// <summary>
            /// Get any flags about the window.
            /// </summary>
            public int Flags;

            /// <summary>
            /// Gets a handle to the show window command.
            /// </summary>
            public int ShowCmd;

            /// <summary>
            /// Gets the minimum position.
            /// </summary>
            public Point MinPosition;

            /// <summary>
            /// Gets the maximum posiiton.
            /// </summary>
            public Point MaxPosition;

            /// <summary>
            /// Gets the normal position.
            /// </summary>
            public Rectangle NormalPosition;
        }
    }
}
