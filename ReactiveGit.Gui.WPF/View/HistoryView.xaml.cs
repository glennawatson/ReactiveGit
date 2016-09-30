namespace ReactiveGit.Gui.WPF.View
{
    using AutoDependencyPropertyMarker;

    using ReactiveGit.Core.Model;
    using ReactiveGit.Gui.Core.ViewModel.CommitHistory;

    using ReactiveUI;

    /// <summary>
    /// Interaction logic for HistoryView.xaml
    /// </summary>
    public partial class HistoryView 
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
                        d(this.Bind(this.ViewModel, vm => vm.SelectedGitObject, view => view.CommitDataGrid.SelectedItem, VmToViewConvert, ViewToVmConvert));
                    });
        }

        private static object VmToViewConvert(IGitIdObject gitObject)
        {
            GitCommit gitCommit = gitObject as GitCommit;

            if (gitCommit == null)
            {
                return null;
            }

            return new CommitHistoryItemViewModel(gitCommit);
        }

        private static IGitIdObject ViewToVmConvert(object value)
        {
            var commitItem = value as CommitHistoryItemViewModel;
            return commitItem == null ? null : commitItem.GitCommit;
        }
    }
}