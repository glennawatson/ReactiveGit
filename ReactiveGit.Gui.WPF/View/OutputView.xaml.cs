// <copyright file="OutputView.xaml.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.WPF.View
{
    using ReactiveUI;

    /// <summary>
    /// Interaction logic for OutputView.xaml
    /// </summary>
    public partial class OutputView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutputView" /> class.
        /// </summary>
        public OutputView()
        {
            this.InitializeComponent();

            this.OneWayBind(this.ViewModel, vm => vm.Output, view => view.OutputTextBox.Text);
        }
    }
}