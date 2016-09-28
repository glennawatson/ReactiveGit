namespace ReactiveGit.Gui.WPF.View
{
    using AutoDependencyPropertyMarker;

    using ReactiveGit.Gui.Core.ViewModel.Output;

    using ReactiveUI;

    /// <summary>
    /// Interaction logic for OutputView.xaml
    /// </summary>
    public partial class OutputView : IViewFor<IOutputViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutputView"/> class.
        /// </summary>
        public OutputView()
        {
            this.InitializeComponent();

            this.OneWayBind(this.ViewModel, vm => vm.Output, view => view.OutputTextBox.Text);
        }

        /// <inheritdoc />
        object IViewFor.ViewModel
        {
            get
            {
                return this.ViewModel;
            }

            set
            {
                this.ViewModel = (IOutputViewModel)value;
            }
        }

        /// <inheritdoc />
        [AutoDependencyProperty]
        public IOutputViewModel ViewModel { get; set; }
    }
}
