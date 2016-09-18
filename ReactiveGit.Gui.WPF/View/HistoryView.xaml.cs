namespace ReactiveGit.Gui.WPF.View
{
    using AutoDependencyPropertyMarker;

    using ReactiveGit.Gui.Base.ViewModel.CommitHistory;

    using ReactiveUI;

    /// <summary>
    /// Interaction logic for HistoryView.xaml
    /// </summary>
    public partial class HistoryView : IViewFor<ICommitHistoryViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryView"/> class.
        /// </summary>
        public HistoryView()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryView"/> class.
        /// </summary>
        /// <param name="viewModel">The view model of the class.</param>
        public HistoryView(ICommitHistoryViewModel viewModel)
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
                this.ViewModel = (ICommitHistoryViewModel)value;
            }
        }

        /// <inheritdoc />
        [AutoDependencyProperty]
        public ICommitHistoryViewModel ViewModel { get; set; }
    }
}