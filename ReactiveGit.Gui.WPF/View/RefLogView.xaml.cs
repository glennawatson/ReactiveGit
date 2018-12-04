// <copyright file="RefLogView.xaml.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.WPF.View
{
    using System;
    using ReactiveUI;

    /// <summary>
    /// Interaction logic for RefLogView.xaml
    /// </summary>
    public partial class RefLogView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefLogView" /> class.
        /// </summary>
        public RefLogView()
        {
            this.InitializeComponent();

            this.WhenActivated(
                d =>
                    {
                        d(this.OneWayBind(this.ViewModel, vm => vm.RefLog, view => view.RefLogDataGrid.ItemsSource));
                        d(this.Bind(this.ViewModel, vm => vm.SelectedGitObject, view => view.RefLogDataGrid.SelectedItem));
                        d(this.WhenAnyValue(x => x.ViewModel.Actions).Subscribe(actions => this.DataItemContextMenu.ItemsSource = actions));
                    });
        }
    }
}