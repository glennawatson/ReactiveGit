namespace ReactiveGit.Gui.WPF.View
{
    using System;

    using ReactiveUI;

    /// <summary>
    /// Interaction logic for CommitDetailsView
    /// </summary>
    public partial class CommitDetailsView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommitDetailsView" /> class.
        /// </summary>
        public CommitDetailsView()
        {
            this.InitializeComponent();

            this.WhenActivated(
                d =>
                    {
                        d(this.Bind(this.ViewModel, vm => vm.DateTime, view => view.DateTextBox.Text, this.VmToViewFunc, this.ViewToVmFunc));
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