// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

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
using ReactiveGit.Gui.Core.ViewModel.Tag;
using ReactiveGit.Gui.WPF.Factories;
using ReactiveGit.Gui.WPF.View;

using ReactiveUI;

using Splat;

namespace ReactiveGit.Gui.WPF.ViewModel
{
    /// <summary>
    /// Main application boot strapper.
    /// </summary>
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        /// <summary>Initializes a new instance of the <see cref="AppBootstrapper"/> class.</summary>
        /// <param name="resolver">The dependency injection resolver which will resolve the programs dependencies.</param>
        /// <param name="dependencyResolver">The dependency injection resolver which will allow registration of new dependencies.</param>
        /// <param name="testRouter">The router.</param>
        public AppBootstrapper(IReadonlyDependencyResolver resolver = null, IMutableDependencyResolver dependencyResolver = null, RoutingState testRouter = null)
        {
            Router = testRouter ?? new RoutingState();
            resolver = resolver ?? Locator.Current;
            dependencyResolver = dependencyResolver ?? Locator.CurrentMutable;

            RegisterParts(dependencyResolver);

            // Navigate to the opening page of the application
            Router.Navigate.Execute(resolver.GetService<IMainViewModel>()).Subscribe();
        }

        /// <inheritdoc />
        public RoutingState Router { get; }

        /// <summary>The register parts.</summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        private void RegisterParts(IMutableDependencyResolver dependencyResolver)
        {
            // Make sure Splat and ReactiveUI are already configured in the locator
            // so that our override runs last
            Locator.CurrentMutable.InitializeSplat();
            Locator.CurrentMutable.InitializeReactiveUI();

            dependencyResolver.RegisterConstant<IScreen>(this);
            dependencyResolver.RegisterConstant<IRepositoryViewModelFactory>(new DefaultRepositoryViewModelFactory());
            dependencyResolver.RegisterConstant<IRepositoryFactory>(new DefaultRepositoryFactory());
            dependencyResolver.RegisterConstant<IWindowLayoutViewModel>(new WindowLayoutViewModel());
            dependencyResolver.Register<ILayoutViewModel>(() => new LayoutViewModel());

            var locator = Locator.Current;

            dependencyResolver.Register<IMainViewModel>(() => new MainViewModel(locator.GetService<IScreen>(), locator.GetService<IRepositoryFactory>(), locator.GetService<IRepositoryViewModelFactory>()));
            dependencyResolver.Register<IViewFor<IMainViewModel>>(() => new MainView());
            dependencyResolver.Register<IViewFor<IBranchViewModel>>(() => new BranchesView());
            dependencyResolver.Register<IViewFor<IRefLogViewModel>>(() => new RefLogView());
            dependencyResolver.Register<IViewFor<ICommitHistoryViewModel>>(() => new HistoryView());
            dependencyResolver.Register<IViewFor<IOutputViewModel>>(() => new OutputView());
            dependencyResolver.Register<IViewFor<IRepositoryDocumentViewModel>>(() => new RepositoryView());
            dependencyResolver.Register<IViewFor<ITagsViewModel>>(() => new TagView());
        }
    }
}