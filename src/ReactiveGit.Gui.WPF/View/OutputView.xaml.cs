// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI;

namespace ReactiveGit.Gui.WPF.View
{
    /// <summary>
    /// Interaction logic for OutputView.xaml.
    /// </summary>
    public partial class OutputView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutputView" /> class.
        /// </summary>
        public OutputView()
        {
            InitializeComponent();

            this.OneWayBind(ViewModel, vm => vm.Output, view => view.OutputTextBox.Text);
        }
    }
}