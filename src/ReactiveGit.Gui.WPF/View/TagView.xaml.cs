// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI;

namespace ReactiveGit.Gui.WPF.View
{
    /// <summary>
    /// Interaction logic for TagView.xaml.
    /// </summary>
    public partial class TagView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagView"/> class.
        /// </summary>
        public TagView()
        {
            InitializeComponent();

            this.WhenActivated(
                d =>
                    {
                        d(this.OneWayBind(ViewModel, vm => vm.Tags, view => view.TagsListBox.ItemsSource));
                        d(this.Bind(ViewModel, vm => vm.SelectedGitObject, view => view.TagsListBox.SelectedItem));
                        d(this.OneWayBind(ViewModel, vm => vm.SelectedTag.Message, view => view.DetailsTextBox.Text));
                    });
        }
    }
}
