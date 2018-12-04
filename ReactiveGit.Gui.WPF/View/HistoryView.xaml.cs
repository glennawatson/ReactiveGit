// <copyright file="HistoryView.xaml.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.WPF.View
{
    using ReactiveGit.Core.Model;
    using ReactiveGit.Gui.Core.ViewModel.CommitHistory;

    using ReactiveUI;

    /// <summary>
    /// Interaction logic for HistoryView.xaml
    /// </summary>
    public partial class HistoryView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryView" /> class.
        /// </summary>
        public HistoryView()
        {
            this.InitializeComponent();

            this.WhenActivated(
                d =>
                    {
                        d(
                            this.OneWayBind(
                                this.ViewModel,
                                vm => vm.CommitHistory,
                                view => view.CommitDataGrid.ItemsSource));
                        d(
                            this.Bind(
                                this.ViewModel,
                                vm => vm.SelectedGitObject,
                                view => view.CommitDataGrid.SelectedItem,
                                VmToViewConvert,
                                ViewToVmConvert));
                    });
        }

        /// <summary>The view to vm convert.</summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="IGitIdObject"/>.</returns>
        private static IGitIdObject ViewToVmConvert(object value)
        {
            var commitItem = value as CommitHistoryItemViewModel;
            return commitItem == null ? null : commitItem.GitCommit;
        }

        /// <summary>The vm to view convert.</summary>
        /// <param name="gitObject">The git object.</param>
        /// <returns>The <see cref="object"/>.</returns>
        private static object VmToViewConvert(IGitIdObject gitObject)
        {
            var gitCommit = gitObject as GitCommit;

            if (gitCommit == null)
            {
                return null;
            }

            return new CommitHistoryItemViewModel(gitCommit);
        }
    }
}