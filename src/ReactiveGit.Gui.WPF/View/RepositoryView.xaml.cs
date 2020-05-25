// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveGit.Gui.Core.ViewModel.Repository;

using ReactiveUI;

namespace ReactiveGit.Gui.WPF.View
{
    /// <summary>
    /// Interaction logic for RepositoryViewModel.xaml.
    /// </summary>
    public partial class RepositoryView : IViewFor<IRepositoryDocumentViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryView" /> class.
        /// </summary>
        public RepositoryView()
        {
            InitializeComponent();

            this.WhenActivated(
                d =>
                    {
                        d(
                            this.OneWayBind(
                                ViewModel,
                                vm => vm.CommitHistoryViewModel,
                                view => view.HistoryView.ViewModel));
                    });
        }
    }
}