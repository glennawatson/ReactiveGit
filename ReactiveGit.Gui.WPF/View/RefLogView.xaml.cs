namespace ReactiveGit.Gui.WPF.View
{
    using AutoDependencyPropertyMarker;

    using ReactiveGit.Gui.Core.ViewModel.RefLog;

    using ReactiveUI;

    /// <summary>
    /// Interaction logic for RefLogView.xaml
    /// </summary>
    public partial class RefLogView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefLogView"/> class.
        /// </summary>
        public RefLogView()
        {
            this.InitializeComponent();

            this.OneWayBind(this.ViewModel, vm => vm.RefLog, view => view.RefLogDataGrid.ItemsSource);
            this.Bind(this.ViewModel, vm => vm.SelectedGitObject, view => view.RefLogDataGrid.SelectedItem);

            this.BindCommand(this.ViewModel, viewModel => viewModel.ResetHard, view => view.ResetHardMenuItem, this.WhenAnyValue(x => x.ViewModel.SelectedGitObject));
            this.BindCommand(this.ViewModel, viewModel => viewModel.ResetMixed, view => view.ResetMixedMenuItem, this.WhenAnyValue(x => x.ViewModel.SelectedGitObject));
            this.BindCommand(this.ViewModel, viewModel => viewModel.ResetSoft, view => view.ResetSoftMenuItem, this.WhenAnyValue(x => x.ViewModel.SelectedGitObject));
        }
    }
}
