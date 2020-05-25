// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

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

namespace ReactiveGit.Gui.WPF.ViewModel
{
    /// <summary>
    /// View model for handling saving and loading the window position data.
    /// </summary>
    public class WindowLayoutViewModel : ReactiveObject, IWindowLayoutViewModel
    {
        private const string _defaultName = "default.windowplacement";
        private const int _showNormal = 1;
        private const int _showMinimised = 2;

        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(SafeNativeMethods.WindowPlacement));

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowLayoutViewModel"/> class.
        /// </summary>
        public WindowLayoutViewModel()
        {
            var isValidWindow = this.WhenAnyValue(x => x.Window).Select(x => x != null);

            Save = ReactiveCommand.Create(SaveImpl, isValidWindow);
            Load = ReactiveCommand.Create(LoadImpl, isValidWindow);
        }

        /// <summary>
        /// Gets or sets the window that the.
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

        private static void LoadPlacement(IntPtr windowHandle, Stream placementStream)
        {
            if (placementStream == null)
            {
                return;
            }

            try
            {
                var placement = (SafeNativeMethods.WindowPlacement)Serializer.Deserialize(placementStream);

                placement.Length = Marshal.SizeOf(typeof(SafeNativeMethods.WindowPlacement));
                placement.Flags = 0;
                placement.ShowCmd = placement.ShowCmd == _showMinimised ? _showNormal : placement.ShowCmd;
                SafeNativeMethods.SetWindowPlacement(windowHandle, ref placement);
            }
            catch (InvalidOperationException)
            {
                // Parsing placement XML failed. Fail silently.
            }
        }

        private static void SavePlacement(IntPtr windowHandle, Stream stream)
        {
            SafeNativeMethods.WindowPlacement placement;
            SafeNativeMethods.GetWindowPlacement(windowHandle, out placement);

            using (var xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8))
            {
                Serializer.Serialize(xmlTextWriter, placement);
            }
        }

        private void SaveImpl()
        {
            try
            {
                var fileStream = IsolatedStorageFile.CreateFile(_defaultName);
                SavePlacement(new WindowInteropHelper(Window).Handle, fileStream);
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
                var filestream = IsolatedStorageFile.OpenFile(_defaultName, FileMode.Open, FileAccess.Read);
                LoadPlacement(new WindowInteropHelper(Window).Handle, filestream);
            }
            catch (Exception)
            {
                // Ignore any errors from loading.
            }
        }
    }
}
