// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

using ReactiveUI;

namespace ReactiveGit.Gui.WPF.View
{
    /// <summary>
    /// Interaction logic for CommitDetailsView.
    /// </summary>
    public partial class CommitDetailsView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommitDetailsView" /> class.
        /// </summary>
        public CommitDetailsView()
        {
            InitializeComponent();

            this.WhenActivated(
                d =>
                    {
                        d(this.Bind(ViewModel, vm => vm.DateTime, view => view.DateTextBox.Text, VmToViewFunc, ViewToVmFunc));
                    });
        }

        private string VmToViewFunc(DateTime dateTime)
        {
            return dateTime.ToString("O"); // return ISO 8601 Date ime
        }

        private DateTime ViewToVmFunc(string value)
        {
            DateTime returnValue;
            DateTime.TryParse(value, out returnValue);
            return returnValue;
        }
    }
}