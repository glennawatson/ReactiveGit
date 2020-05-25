// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using ReactiveUI;

namespace ReactiveGit.Gui.WPF.View
{
    /// <summary>
    /// Interaction logic for RefLogView.xaml.
    /// </summary>
    public partial class RefLogView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefLogView" /> class.
        /// </summary>
        public RefLogView()
        {
            InitializeComponent();

            this.WhenActivated(
                d =>
                    {
                        d(this.OneWayBind(ViewModel, vm => vm.RefLog, view => view.RefLogDataGrid.ItemsSource));
                        d(this.Bind(ViewModel, vm => vm.SelectedGitObject, view => view.RefLogDataGrid.SelectedItem));
                        d(this.WhenAnyValue(x => x.ViewModel.Actions).Subscribe(actions => DataItemContextMenu.ItemsSource = actions));
                    });
        }
    }
}