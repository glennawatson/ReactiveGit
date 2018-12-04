// <copyright file="WindowLayoutViewModel.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.WPF.ViewModel
{
    using System;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interop;
    using System.Xml;
    using System.Xml.Serialization;

    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;

    /// <summary>
    /// View model for handling saving and loading the window position data.
    /// </summary>
    public class WindowLayoutViewModel : ReactiveObject, IWindowLayoutViewModel
    {
        private const string DefaultName = "default.windowplacement";
        private const int ShowNormal = 1;
        private const int ShowMinimised = 2;

        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(WindowPlacement));

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowLayoutViewModel"/> class.
        /// </summary>
        public WindowLayoutViewModel()
        {
            var isValidWindow = this.WhenAnyValue(x => x.Window).Select(x => x != null);

            this.Save = ReactiveCommand.Create(this.SaveImpl, isValidWindow);
            this.Load = ReactiveCommand.Create(this.LoadImpl, isValidWindow);
        }

        /// <summary>
        /// Gets or sets the window that the 
        /// </summary>
        [Reactive]
        public Window Window { get; set; }

        /// <summary>
        /// Gets a command that will save the window placement data.
        /// </summary>
        public ICommand Save { get; }

        /// <summary>
        /// Gets a command that will load the window placement data.
        /// </summary>
        public ICommand Load { get; }

        private static IsolatedStorageFile IsolatedStorageFile => IsolatedStorageFile.GetUserStoreForAssembly();

        [DllImport("user32.dll")]
        private static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WindowPlacement lpwndpl);

        [DllImport("user32.dll")]
        private static extern bool GetWindowPlacement(IntPtr hWnd, out WindowPlacement lpwndpl);

        private static void LoadPlacement(IntPtr windowHandle, Stream placementStream)
        {
            if (placementStream == null)
            {
                return;
            }

            try
            {
                var placement = (WindowPlacement)Serializer.Deserialize(placementStream);

                placement.Length = Marshal.SizeOf(typeof(WindowPlacement));
                placement.Flags = 0;
                placement.ShowCmd = placement.ShowCmd == ShowMinimised ? ShowNormal : placement.ShowCmd;
                SetWindowPlacement(windowHandle, ref placement);
            }
            catch (InvalidOperationException)
            {
                // Parsing placement XML failed. Fail silently.
            }
        }

        private static void SavePlacement(IntPtr windowHandle, Stream stream)
        {
            WindowPlacement placement;
            GetWindowPlacement(windowHandle, out placement);

            using (var xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8))
            {
                Serializer.Serialize(xmlTextWriter, placement);
            }
        }

        private void SaveImpl()
        {
            try
            {
                var fileStream = IsolatedStorageFile.CreateFile(DefaultName);
                SavePlacement(new WindowInteropHelper(this.Window).Handle, fileStream);
            }
            catch (Exception)
            {
                // Ignore any errors from saving.
            }
        }

        private void LoadImpl()
        {
            try
            {
                var filestream = IsolatedStorageFile.OpenFile(DefaultName, FileMode.Open, FileAccess.Read);
                LoadPlacement(new WindowInteropHelper(this.Window).Handle, filestream);
            }
            catch (Exception)
            {
                // Ignore any errors from loading.
            }
        }

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
            /// Initializes a new instance of the <see cref="Rectangle"/> struct.
            /// </summary>
            /// <param name="left">The left of the window.</param>
            /// <param name="top">The top of the window.</param>
            /// <param name="right">The right of the window.</param>
            /// <param name="bottom">The bottom of the window.</param>
            public Rectangle(int left, int top, int right, int bottom)
            {
                this.Left = left;
                this.Top = top;
                this.Right = right;
                this.Bottom = bottom;
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
            /// Initializes a new instance of the <see cref="Point"/> struct.
            /// </summary>
            /// <param name="x">The x coordinate.</param>
            /// <param name="y">The y coordinate.</param>
            public Point(int x, int y)
            {
                this.X = x;
                this.Y = y;
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
