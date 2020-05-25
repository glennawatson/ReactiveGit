// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveGit.Gui.Core.ViewModel.CommitHistory;
using ReactiveGit.Library.Core.Model;
using ReactiveUI;

namespace ReactiveGit.Gui.WPF.View
{
    /// <summary>
    /// Interaction logic for HistoryView.xaml.
    /// </summary>
    public partial class HistoryView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryView" /> class.
        /// </summary>
        public HistoryView()
        {
            InitializeComponent();

            this.WhenActivated(
                d =>
                    {
                        d(
                            this.OneWayBind(
                                ViewModel,
                                vm => vm.CommitHistory,
                                view => view.CommitDataGrid.ItemsSource));
                        d(
                            this.Bind(
                                ViewModel,
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