// <copyright file="MainViewModel.cs" company="Glenn Watson">
// Copyright (c) Glenn Watson. All rights reserved.
// </copyright>
namespace ReactiveGit.Gui.Core.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;
    using System.Reflection;
    using System.Windows.Input;

    using ReactiveGit.Gui.Core.ExtensionMethods;
    using ReactiveGit.Gui.Core.Model;
    using ReactiveGit.Gui.Core.Model.Factories;
    using ReactiveGit.Gui.Core.ViewModel.Factories;
    using ReactiveGit.Gui.Core.ViewModel.Repository;

    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;

    /// <summary>
    /// The main view model.
    /// </summary>
    public class MainViewModel : ReactiveObject, IMainViewModel
    {
        private readonly ReactiveCommand<Unit, string> newRepository;

        private readonly IRepositoryFactory repositoryFactory;

        private readonly ReactiveList<IRepositoryDocumentViewModel> repositoryViewModels =
            new ReactiveList<IRepositoryDocumentViewModel>();

        private readonly ReactiveCommand<Unit, string> selectRepository;

        private readonly IRepositoryViewModelFactory viewModelFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel" /> class.
        /// </summary>
        /// <param name="screen">The screen in which the view is shown.</param>
        /// <param name="repositoryFactory">Factory for creating the repository details.</param>
        /// <param name="viewModelFactory">Factory for creating the document view models.</param>
        public MainViewModel(
            IScreen screen,
            IRepositoryFactory repositoryFactory,
            IRepositoryViewModelFactory viewModelFactory)
        {
            if (screen == null)
            {
                throw new ArgumentNullException(nameof(screen));
            }

            if (repositoryFactory == null)
            {
                throw new ArgumentNullException(nameof(repositoryFactory));
            }

            if (viewModelFactory == null)
            {
                throw new ArgumentNullException(nameof(viewModelFactory));
            }

            this.HostScreen = screen;
            this.repositoryFactory = repositoryFactory;
            this.viewModelFactory = viewModelFactory;

            this.SelectFolder = new Interaction<string, string>();

            this.selectRepository =
                ReactiveCommand.CreateFromObservable(() => this.SelectFolder.Handle("Select folder for GIT repository"));
            this.selectRepository.Subscribe(this.OpenRepository);

            this.newRepository =
                ReactiveCommand.CreateFromObservable(
                    () => this.SelectFolder.Handle("Select new folder for GIT repository"));

            this.newRepository.Subscribe(
                x =>
                    repositoryFactory.CreateRepositoryCreator().CreateRepository(x).Subscribe(
                        _ => { this.OpenRepository(x); }));

            this.AllSupportViewModels = this.GetAllSupportViewModels();

            this.VisibleSupportViewModels = new ReactiveList<ISupportViewModel>(this.AllSupportViewModels);

            this.WhenAnyValue(x => x.SelectedRepositoryViewModel).Subscribe(this.UpdateRepositoryDetails);
        }

        /// <inheritdoc />
        public IReadOnlyList<ISupportViewModel> AllSupportViewModels { get; }

        /// <inheritdoc />
        public IScreen HostScreen { get; }

        /// <inheritdoc />
        public ICommand NewRepository => this.newRepository;

        /// <inheritdoc />
        public IReadOnlyList<IRepositoryDocumentViewModel> RepositoryViewModels => this.repositoryViewModels;

        /// <summary>
        /// Gets or sets the selected view model.
        /// </summary>
        [Reactive]
        public IRepositoryDocumentViewModel SelectedRepositoryViewModel { get; set; }

        /// <inheritdoc />
        public Interaction<string, string> SelectFolder { get; }

        /// <summary>
        /// Gets a command that will select the repository.
        /// </summary>
        public ICommand SelectRepository => this.selectRepository;

        /// <inheritdoc />
        public string UrlPathSegment => string.Empty;

        /// <inheritdoc />
        public IList<ISupportViewModel> VisibleSupportViewModels { get; }

        private IReadOnlyList<ISupportViewModel> GetAllSupportViewModels()
        {
            IEnumerable<Type> supportViewModelTypes =
                typeof(ISupportViewModel).GetTypeInfo().Assembly.ExportedTypes.Where(
                    x =>
                        x.GetTypeInfo().ImplementedInterfaces.Contains(typeof(ISupportViewModel))
                        && (x.GetTypeInfo().IsAbstract == false));

            return supportViewModelTypes.Select(type => Activator.CreateInstance(type) as ISupportViewModel).ToList();
        }

        /// <summary>
        /// Opens a repository and creates all the view models.
        /// </summary>
        /// <param name="repositoryDirectoryPath">The path to the repository to open.</param>
        private void OpenRepository(string repositoryDirectoryPath)
        {
            if (string.IsNullOrWhiteSpace(repositoryDirectoryPath))
            {
                return;
            }

            IRepositoryDetails newModel = this.repositoryFactory.CreateRepositoryDetails(repositoryDirectoryPath);
            this.repositoryViewModels.Add(this.viewModelFactory.CreateRepositoryViewModel(newModel));
            this.SelectedRepositoryViewModel = this.repositoryViewModels.Last();
        }

        private void UpdateRepositoryDetails(IRepositoryDocumentViewModel documentViewModel)
        {
            foreach (ISupportViewModel supportViewModel in this.VisibleSupportViewModels)
            {
                supportViewModel.RepositoryDetails = documentViewModel?.RepositoryDetails;

                var refreshableViewModel = supportViewModel as IRefreshableViewModel;
                refreshableViewModel?.Refresh.InvokeCommand();
            }
        }
    }
}