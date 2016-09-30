namespace ReactiveGit.Gui.WPF.View
{
    using AutoDependencyPropertyMarker;

    using ReactiveGit.Gui.Core.ViewModel.Output;

    using ReactiveUI;

    /// <summary>
    /// Interaction logic for OutputView.xaml
    /// </summary>
    public partial class OutputView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutputView"/> class.
        /// </summary>
        public OutputView()
        {
            this.InitializeComponent();

            this.OneWayBind(this.ViewModel, vm => vm.Output, view => view.OutputTextBox.Text);
        }
    }
}
