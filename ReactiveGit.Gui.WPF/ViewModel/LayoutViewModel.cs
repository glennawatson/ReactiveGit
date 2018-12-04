// <copyright file="LayoutViewModel.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

using DynamicData.Binding;

namespace ReactiveGit.Gui.WPF.ViewModel
{
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

    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;

    using Xceed.Wpf.AvalonDock;
    using Xceed.Wpf.AvalonDock.Layout.Serialization;

    /// <summary>
    /// View model for handling saving and loading docking layouts.
    /// </summary>
    public class LayoutViewModel : ReactiveObject, ILayoutViewModel
    {
        private static readonly string DefaultLayout = "default.layout";

        private static readonly string CurrentLayout = "current.layout";

        private readonly ObservableCollectionExtended<string> layoutFileNames = new ObservableCollectionExtended<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutViewModel"/> class.
        /// </summary>
        public LayoutViewModel()
        {
            this.Start = ReactiveCommand.Create(this.StartImpl);
            this.Shutdown = ReactiveCommand.Create(() => this.PeformSaveLayout(IsolatedStorageFile.CreateFile(CurrentLayout)));

            var canSave = this.WhenAnyValue(x => x.SelectedLayout).Select(x => string.IsNullOrWhiteSpace(x) == false);

            this.SaveLayout = ReactiveCommand.Create<string>(layoutName => this.PeformSaveLayout(IsolatedStorageFile.CreateFile(layoutName)), canSave);

            this.ResetLayout = ReactiveCommand.Create(this.ResetImpl);
        }

        /// <inheritdoc />
        [Reactive]
        public DockingManager DockingManager { get; set; }

        /// <inheritdoc />
        [Reactive]
        public string SelectedLayout { get; set; }

        /// <inheritdoc />
        public IEnumerable<string> SavedLayoutNames => this.layoutFileNames;

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
                this.GetLayoutFileNames();
                this.PerformLoadLayout(IsolatedStorageFile.OpenFile(CurrentLayout, FileMode.Open, FileAccess.Read));
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

            this.PerformLoadLayout(streamInfo.Stream);
        }

        private void PerformLoadLayout(Stream stream)
        {
            if (stream == null)
            {
                return;
            }

            var layoutSerializer = new XmlLayoutSerializer(this.DockingManager);

            layoutSerializer.Deserialize(stream);
        }

        private void PeformSaveLayout(Stream stream)
        {
            if (stream == null)
            {
                return;
            }

            var layoutSerializer = new XmlLayoutSerializer(this.DockingManager);

            layoutSerializer.Serialize(stream);
        }

        private void GetLayoutFileNames()
        {
            this.layoutFileNames.AddRange(IsolatedStorageFile.GetFileNames("*.layout"));
        }
    }
}
