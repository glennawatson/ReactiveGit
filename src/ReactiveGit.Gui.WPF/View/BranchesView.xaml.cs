// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI;

namespace ReactiveGit.Gui.WPF.View
{
    /// <summary>
    /// Interaction logic for BranchesView.xaml.
    /// </summary>
    public partial class BranchesView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BranchesView" /> class.
        /// </summary>
        public BranchesView()
        {
            InitializeComponent();
            this.WhenAnyValue(x => x.ViewModel).BindTo(this, x => x.DataContext);
        }
    }
}