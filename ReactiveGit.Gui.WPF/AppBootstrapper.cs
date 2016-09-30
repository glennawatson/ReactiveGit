namespace ReactiveGit.Gui.WPF
{
    using System;

    using ReactiveGit.Gui.Core.ExtensionMethods;
    using ReactiveGit.Gui.Core.Model.Factories;
    using ReactiveGit.Gui.Core.ViewModel;
    using ReactiveGit.Gui.Core.ViewModel.Branches;
    using ReactiveGit.Gui.Core.ViewModel.CommitHistory;
    using ReactiveGit.Gui.Core.ViewModel.Factories;
    using ReactiveGit.Gui.Core.ViewModel.Output;
    using ReactiveGit.Gui.Core.ViewModel.RefLog;
    using ReactiveGit.Gui.Core.ViewModel.Repository;
    using ReactiveGit.Gui.WPF.Factories;
    using ReactiveGit.Gui.WPF.View;

    using ReactiveUI;

    using Splat;

    /// <summary>
    /// Main application boot strapper.
    /// </summary>
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        /// <summary>Initializes a new instance of the <see cref="AppBootstrapper"/> class.</summary>
        /// <param name="dependencyResolver">The dependency injection resolver which will resolve the programs dependencies.</param>
        /// <param name="testRouter">The router.</param>
        public AppBootstrapper(IMutableDependencyResolver dependencyResolver = null, RoutingState testRouter = null)
        {
            this.Router = testRouter ?? new RoutingState();
            dependencyResolver = dependencyResolver ?? Locator.CurrentMutable;

            this.RegisterParts(dependencyResolver);

            LogHost.Default.Level = LogLevel.Debug;

            // Navigate to the opening page of the application
            this.Router.Navigate.Execute(dependencyResolver.GetService<IMainViewModel>()).Subscribe();
        }

        /// <inheritdoc />
        public RoutingState Router { get; }

        /// <summary>The register parts.</summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        private void RegisterParts(IMutableDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterConstant<IScreen>(this);
            dependencyResolver.RegisterConstant<IRepositoryViewModelFactory>(new DefaultRepositoryViewModelFactory());
            dependencyResolver.RegisterConstant<IRepositoryFactory>(new DefaultRepositoryFactory());

            dependencyResolver.Register<MainViewModel, IMainViewModel>();

            dependencyResolver.Register<IViewFor<MainViewModel>>(() => new MainView());
            dependencyResolver.Register<IViewFor<BranchViewModel>>(() => new BranchesView());
            dependencyResolver.Register<IViewFor<RefLogViewModel>>(() => new RefLogView());
            dependencyResolver.Register<IViewFor<CommitHistoryViewModel>>(() => new HistoryView());
            dependencyResolver.Register<IViewFor<OutputViewModel>>(() => new OutputView());
            dependencyResolver.Register<IViewFor<RepositoryDocumentViewModel>>(() => new RepositoryView());
        }
    }
}