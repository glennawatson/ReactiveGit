namespace ReactiveGit.Gui.WPF.View
{
    using AutoDependencyPropertyMarker;

    using Microsoft.WindowsAPICodePack.Dialogs;

    using ReactiveGit.Gui.Core.ViewModel;
    using ReactiveGit.Gui.WPF.SplatConverters;

    using ReactiveUI;

    /// <summary>
    /// Interaction logic for MainView
    /// </summary>
    public partial class MainView : IViewFor<IMainViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainView"/> class.
        /// </summary>
        public MainView()
        {
            this.InitializeComponent();

            this.WhenActivated(
                d =>
                {
                    d(this.ViewModel.SelectFolder.RegisterHandler(context => this.HandleFolderPrompt(context)));
                    d(this.BindCommand(this.ViewModel, vm => vm.NewRepository, view => view.NewMenuItem));
                    d(this.BindCommand(this.ViewModel, vm => vm.SelectRepository, view => view.OpenMenuItem));
                    d(this.OneWayBind(this.ViewModel, vm => vm.RepositoryViewModels, view => view.DockManager.DocumentsSource));
                    d(this.Bind(this.ViewModel, vm => vm.SelectedRepositoryViewModel, view => view.DockManager.ActiveContent, viewToVMConverterOverride: new DocumentTypeConverter()));
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
                this.ViewModel = (IMainViewModel)value;
            }
        }

        /// <inheritdoc />
        [AutoDependencyProperty]
        public IMainViewModel ViewModel { get; set; }

        /// <summary>
        /// Handles the folder prompt.
        /// </summary>
        /// <param name="context">The context of the interaction.</param>
        private void HandleFolderPrompt(InteractionContext<string, string> context)
        {
            var dialog = new CommonOpenFileDialog { IsFolderPicker = true, Title = context.Input };

            context.SetOutput(dialog.ShowDialog() != CommonFileDialogResult.Ok ? null : dialog.FileName);
        }
    }
}
