namespace ReactiveGit.Gui.WPF.View
{
    using AutoDependencyPropertyMarker;

    using ReactiveGit.Gui.Base.ViewModel.RefLog;

    using ReactiveUI;

    /// <summary>
    /// Interaction logic for RefLogView.xaml
    /// </summary>
    public partial class RefLogView : IViewFor<IRefLogViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefLogView"/> class.
        /// </summary>
        public RefLogView()
        {
            this.InitializeComponent();
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
                this.ViewModel = (IRefLogViewModel)value;
            }
        }

        /// <inheritdoc />
        [AutoDependencyProperty]
        public IRefLogViewModel ViewModel { get; set; }
    }
}
