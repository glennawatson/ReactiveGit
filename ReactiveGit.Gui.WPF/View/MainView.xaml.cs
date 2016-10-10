namespace ReactiveGit.Gui.WPF.View
{
    using System;
    using System.Reactive.Disposables;

    using Microsoft.WindowsAPICodePack.Dialogs;

    using ReactiveGit.Gui.Core.ExtensionMethods;
    using ReactiveGit.Gui.WPF.Converters;
    using ReactiveGit.Gui.WPF.ViewModel;

    using ReactiveUI;

    using Splat;

    /// <summary>
    /// Interaction logic for MainView
    /// </summary>
    public partial class MainView 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainView" /> class.
        /// </summary>
        public MainView()
        {
            this.InitializeComponent();

            this.LayoutViewModel = Locator.Current.GetService<ILayoutViewModel>();

            this.WhenActivated(
                d =>
                    {
                        d(this.WhenAnyValue(x => x.DockManager).BindTo(this.LayoutViewModel, vm => vm.DockingManager));
                        d(this.ViewModel.SelectFolder.RegisterHandler(context => this.HandleFolderPrompt(context)));
                        d(this.BindCommand(this.ViewModel, vm => vm.NewRepository, view => view.NewMenuItem));
                        d(this.BindCommand(this.ViewModel, vm => vm.SelectRepository, view => view.OpenMenuItem));
                        d(
                            this.OneWayBind(
                                this.ViewModel,
                                vm => vm.RepositoryViewModels,
                                view => view.DockManager.DocumentsSource));
                        d(
                            this.Bind(
                                this.ViewModel,
                                vm => vm.SelectedRepositoryViewModel,
                                view => view.DockManager.ActiveContent,
                                viewToVMConverterOverride: new DocumentTypeConverter()));
                        d(
                            this.OneWayBind(
                                this.ViewModel,
                                vm => vm.VisibleSupportViewModels,
                                view => view.DockManager.AnchorablesSource));
                        d(this.HandleLayout());
                        d(this.WhenAnyValue(x => x.LayoutViewModel.ResetLayout).BindTo(this, view => view.ResetWindowLayout.Command));
                    });
        }

        /// <summary>
        /// Gets the layout view model. It is responsible for saving and loading the layout.
        /// </summary>
        public ILayoutViewModel LayoutViewModel { get; }

        /// <summary>Handles the folder prompt.</summary>
        /// <param name="context">The context of the interaction.</param>
        private void HandleFolderPrompt(InteractionContext<string, string> context)
        {
            var dialog = new CommonOpenFileDialog { IsFolderPicker = true, Title = context.Input };

            context.SetOutput(dialog.ShowDialog() != CommonFileDialogResult.Ok ? null : dialog.FileName);
        }

        private IDisposable HandleLayout()
        {
            this.LayoutViewModel.Start.InvokeCommand();

            return Disposable.Create(() => this.LayoutViewModel.Shutdown.InvokeCommand());
        }
    }
}