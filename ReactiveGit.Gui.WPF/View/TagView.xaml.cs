// <copyright file="TagView.xaml.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.WPF.View
{
    using ReactiveUI;

    /// <summary>
    /// Interaction logic for TagView.xaml
    /// </summary>
    public partial class TagView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagView"/> class.
        /// </summary>
        public TagView()
        {
            this.InitializeComponent();

            this.WhenActivated(
                d =>
                    {
                        d(this.OneWayBind(this.ViewModel, vm => vm.Tags, view => view.TagsListBox.ItemsSource));
                        d(this.Bind(this.ViewModel, vm => vm.SelectedGitObject, view => view.TagsListBox.SelectedItem));
                        d(this.OneWayBind(this.ViewModel, vm => vm.SelectedTag.Message, view => view.DetailsTextBox.Text));
                    });
        }
    }
}
