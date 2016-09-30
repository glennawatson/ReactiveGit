namespace ReactiveGit.Gui.WPF.View
{
    using ReactiveGit.Gui.Core.ViewModel.Repository;

    using ReactiveUI;

    /// <summary>
    /// Interaction logic for RepositoryViewModel.xaml
    /// </summary>
    public partial class RepositoryView : IViewFor<IRepositoryDocumentViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryView"/> class.
        /// </summary>
        public RepositoryView()
        {
            this.InitializeComponent();

            this.WhenActivated(
                d =>
                    {
                        d(this.OneWayBind(this.ViewModel, vm => vm.CommitHistoryViewModel, view => view.HistoryView.ViewModel));
                    });
        }
    }
}
