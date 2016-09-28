namespace ReactiveGit.Gui.WPF.View
{
    using AutoDependencyPropertyMarker;

    using ReactiveGit.Gui.Core.ViewModel.Branches;

    using ReactiveUI;

    /// <summary>
    /// Interaction logic for BranchesView.xaml
    /// </summary>
    public partial class BranchesView 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BranchesView"/> class.
        /// </summary>
        public BranchesView()
        {
            this.InitializeComponent();
            this.WhenAnyValue(x => x.ViewModel).BindTo(this, x => x.DataContext);
        }
    }
}
