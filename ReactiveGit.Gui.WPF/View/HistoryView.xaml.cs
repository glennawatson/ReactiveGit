namespace ReactiveGit.Gui.WPF.View
{
    using AutoDependencyPropertyMarker;

    using ReactiveGit.Gui.Core.ViewModel.CommitHistory;

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
        {
            this.InitializeComponent();

            this.WhenActivated(
                d =>
                    {
                        d(this.OneWayBind(this.ViewModel, vm => vm.CommitHistory, view => view.CommitDataGrid.ItemsSource));
                        d(this.Bind(this.ViewModel, vm => vm.SelectedGitObject, view => view.CommitDataGrid.SelectedItem));
                    });
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