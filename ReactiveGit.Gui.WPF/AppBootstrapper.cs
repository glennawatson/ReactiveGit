namespace ReactiveGit.Gui.WPF
{
    using ReactiveGit.Gui.Core.ExtensionMethods;
    using ReactiveGit.Gui.Core.Model.Factories;
    using ReactiveGit.Gui.Core.ViewModel;
    using ReactiveGit.Gui.Core.ViewModel.Factories;
    using ReactiveGit.Gui.WPF.Factories;
    using ReactiveGit.Gui.WPF.View;

    using ReactiveUI;
    using Splat;

    /// <summary>
    /// Main application boot strapper. 
    /// </summary>
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppBootstrapper"/> class.
        /// </summary>
        /// <param name="dependencyResolver">The dependency injection resolver which will resolve the programs dependencies.</param>
        /// <param name="testRouter">The router.</param>
        public AppBootstrapper(IMutableDependencyResolver dependencyResolver = null, RoutingState testRouter = null)
        {
            this.Router = testRouter ?? new RoutingState();
            dependencyResolver = dependencyResolver ?? Locator.CurrentMutable;

            this.RegisterParts(dependencyResolver);

            LogHost.Default.Level = LogLevel.Debug;

            // Navigate to the opening page of the application
            this.Router.Navigate.Execute(dependencyResolver.GetService<IMainViewModel>());
        }

        /// <inheritdoc />
        public RoutingState Router { get; }

        private void RegisterParts(IMutableDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterConstant<IScreen>(this);
            dependencyResolver.RegisterConstant<IRepositoryViewModelFactory>(new DefaultRepositoryViewModelFactory());
            dependencyResolver.RegisterConstant<IRepositoryFactory>(new DefaultRepositoryFactory());

            dependencyResolver.Register<MainViewModel, IMainViewModel>();

            dependencyResolver.Register<IViewFor<MainViewModel>>(() => new MainView());
        }
    }
}
