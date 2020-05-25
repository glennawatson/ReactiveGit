// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reflection;
using System.Windows.Input;
using DynamicData.Binding;
using ReactiveGit.Gui.Core.ExtensionMethods;
using ReactiveGit.Gui.Core.Model;
using ReactiveGit.Gui.Core.Model.Factories;
using ReactiveGit.Gui.Core.ViewModel.Factories;
using ReactiveGit.Gui.Core.ViewModel.Repository;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ReactiveGit.Gui.Core.ViewModel
{
    /// <summary>
    /// The main view model.
    /// </summary>
    public class MainViewModel : ReactiveObject, IMainViewModel
    {
        private readonly ReactiveCommand<Unit, string> _newRepository;

        private readonly IRepositoryFactory _repositoryFactory;

        private readonly ObservableCollectionExtended<IRepositoryDocumentViewModel> _repositoryViewModels =
            new ObservableCollectionExtended<IRepositoryDocumentViewModel>();

        private readonly ReactiveCommand<Unit, string> _selectRepository;

        private readonly IRepositoryViewModelFactory _viewModelFactory;

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
            HostScreen = screen ?? throw new ArgumentNullException(nameof(screen));
            _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            _viewModelFactory = viewModelFactory ?? throw new ArgumentNullException(nameof(viewModelFactory));

            SelectFolder = new Interaction<string, string>();

            _selectRepository =
                ReactiveCommand.CreateFromObservable(() => SelectFolder.Handle("Select folder for GIT repository"));
            _selectRepository.Subscribe(OpenRepository);

            _newRepository =
                ReactiveCommand.CreateFromObservable(
                    () => SelectFolder.Handle("Select new folder for GIT repository"));

            _newRepository.Subscribe(
                x =>
                    repositoryFactory.CreateRepositoryCreator().CreateRepository(x).Subscribe(
                        _ => { OpenRepository(x); }));

            AllSupportViewModels = GetAllSupportViewModels();

            VisibleSupportViewModels = new ObservableCollectionExtended<ISupportViewModel>(AllSupportViewModels);

            this.WhenAnyValue(x => x.SelectedRepositoryViewModel).Subscribe(UpdateRepositoryDetails);
        }

        /// <inheritdoc />
        public IReadOnlyList<ISupportViewModel> AllSupportViewModels { get; }

        /// <inheritdoc />
        public IScreen HostScreen { get; }

        /// <inheritdoc />
        public ICommand NewRepository => _newRepository;

        /// <inheritdoc />
        public IReadOnlyList<IRepositoryDocumentViewModel> RepositoryViewModels => _repositoryViewModels;

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
        public ICommand SelectRepository => _selectRepository;

        /// <inheritdoc />
        public string UrlPathSegment => string.Empty;

        /// <inheritdoc />
        public IList<ISupportViewModel> VisibleSupportViewModels { get; }

        private static IReadOnlyList<ISupportViewModel> GetAllSupportViewModels()
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

            IRepositoryDetails newModel = _repositoryFactory.CreateRepositoryDetails(repositoryDirectoryPath);
            _repositoryViewModels.Add(_viewModelFactory.CreateRepositoryViewModel(newModel));
            SelectedRepositoryViewModel = _repositoryViewModels.Last();
        }

        private void UpdateRepositoryDetails(IRepositoryDocumentViewModel documentViewModel)
        {
            foreach (ISupportViewModel supportViewModel in VisibleSupportViewModels)
            {
                supportViewModel.RepositoryDetails = documentViewModel?.RepositoryDetails;

                var refreshableViewModel = supportViewModel as IRefreshableViewModel;
                refreshableViewModel?.Refresh.InvokeCommand();
            }
        }
    }
}