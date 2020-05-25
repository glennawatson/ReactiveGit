// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Resources;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout.Serialization;

namespace ReactiveGit.Gui.WPF.ViewModel
{
    /// <summary>
    /// View model for handling saving and loading docking layouts.
    /// </summary>
    public class LayoutViewModel : ReactiveObject, ILayoutViewModel
    {
        private static readonly string DefaultLayout = "default.layout";

        private static readonly string CurrentLayout = "current.layout";

        private readonly ObservableCollectionExtended<string> _layoutFileNames = new ObservableCollectionExtended<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutViewModel"/> class.
        /// </summary>
        public LayoutViewModel()
        {
            Start = ReactiveCommand.Create(StartImpl);
            Shutdown = ReactiveCommand.Create(() => PeformSaveLayout(IsolatedStorageFile.CreateFile(CurrentLayout)));

            var canSave = this.WhenAnyValue(x => x.SelectedLayout).Select(x => string.IsNullOrWhiteSpace(x) == false);

            SaveLayout = ReactiveCommand.Create<string>(layoutName => PeformSaveLayout(IsolatedStorageFile.CreateFile(layoutName)), canSave);

            ResetLayout = ReactiveCommand.Create(ResetImpl);
        }

        /// <inheritdoc />
        [Reactive]
        public DockingManager DockingManager { get; set; }

        /// <inheritdoc />
        [Reactive]
        public string SelectedLayout { get; set; }

        /// <inheritdoc />
        public IEnumerable<string> SavedLayoutNames => _layoutFileNames;

        /// <inheritdoc />
        public ICommand OpenLayout { get; }

        /// <inheritdoc />
        public ICommand SaveLayout { get; }

        /// <inheritdoc />
        public ICommand ResetLayout { get; }

        /// <inheritdoc />
        public ICommand Start { get; }

        /// <inheritdoc />
        public ICommand Shutdown { get; }

        private static IsolatedStorageFile IsolatedStorageFile => IsolatedStorageFile.GetUserStoreForAssembly();

        private void StartImpl()
        {
            try
            {
                GetLayoutFileNames();
                PerformLoadLayout(IsolatedStorageFile.OpenFile(CurrentLayout, FileMode.Open, FileAccess.Read));
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void ResetImpl()
        {
            StreamResourceInfo streamInfo =
                Application.GetResourceStream(
                    new Uri("pack://application:,,,/Resources/DefaultLayout.xml", UriKind.Absolute));

            if (streamInfo == null)
            {
                throw new Exception("Cannot find the default layout.");
            }

            PerformLoadLayout(streamInfo.Stream);
        }

        private void PerformLoadLayout(Stream stream)
        {
            if (stream == null)
            {
                return;
            }

            var layoutSerializer = new XmlLayoutSerializer(DockingManager);

            layoutSerializer.Deserialize(stream);
        }

        private void PeformSaveLayout(Stream stream)
        {
            if (stream == null)
            {
                return;
            }

            var layoutSerializer = new XmlLayoutSerializer(DockingManager);

            layoutSerializer.Serialize(stream);
        }

        private void GetLayoutFileNames()
        {
            _layoutFileNames.AddRange(IsolatedStorageFile.GetFileNames("*.layout"));
        }
    }
}
