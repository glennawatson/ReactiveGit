// <copyright file="MainViewModel.cs" company="Glenn Watson">
// Copyright (c) Glenn Watson. All rights reserved.
// </copyright>
namespace ReactiveGit.Gui.Base.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Windows.Input;

    using ReactiveGit.Gui.Base.Model.Factories;
    using ReactiveGit.Gui.Base.View;
    using ReactiveGit.Gui.Base.ViewModel.Factories;
    using ReactiveGit.Gui.Base.ViewModel.Repository;

    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;

    /// <summary>
    /// The main view model.
    /// </summary>
    public class MainViewModel : ReactiveObject, IMainViewModel
    {
        private readonly IFolderSelector folderSelector;

        private readonly IRepositoryFactory repositoryFactory;

        private readonly IRepositoryViewModelFactory viewModelFactory;

        private readonly ReactiveList<IRepositoryDocumentViewModel> repositoryViewModels;

        private readonly ReactiveCommand<Unit, string> selectRepository;

        private readonly ReactiveCommand<Unit, string> newRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="folderSelector">A class for generating folders.</param>
        /// <param name="repositoryFactory">Factory for creating the repository details.</param>
        /// <param name="viewModelFactory">Factory for creating the document view models.</param>
        public MainViewModel(IFolderSelector folderSelector, IRepositoryFactory repositoryFactory, IRepositoryViewModelFactory viewModelFactory)
        {
            this.repositoryFactory = repositoryFactory;
            this.folderSelector = folderSelector;
            this.viewModelFactory = viewModelFactory;

            this.selectRepository = ReactiveCommand.CreateFromObservable(() => this.folderSelector.Prompt());
            this.selectRepository.Subscribe(this.OpenRepository);

            this.newRepository = ReactiveCommand.CreateFromObservable(() => this.folderSelector.Prompt());
            this.newRepository.Subscribe(x =>
                {
                    repositoryFactory.CreateRepositoryCreator().CreateRepository(x).Subscribe(_ =>
                        {
                            this.OpenRepository(x);
                        });                    
                });

            this.repositoryViewModels = new ReactiveList<IRepositoryDocumentViewModel>();
        }

        /// <summary>
        /// Gets a command that will select the repository.
        /// </summary>
        public ICommand SelectRepository => this.selectRepository;

        /// <inheritdoc />
        public ICommand NewRepository => this.newRepository;

        /// <inheritdoc />
        public IReadOnlyList<IRepositoryDocumentViewModel> RepositoryViewModels => this.repositoryViewModels;

        /// <summary>
        /// Gets or sets the selected view model.
        /// </summary>
        [Reactive]
        public IRepositoryDocumentViewModel SelectedRepositoryViewModel { get; set; }

        private void OpenRepository(string repositoryDirectoryPath)
        {
            var newModel = this.repositoryFactory.CreateRepositoryDetails(repositoryDirectoryPath);
            this.repositoryViewModels.Add(this.viewModelFactory.CreateRepositoryViewModel(newModel));
            this.SelectedRepositoryViewModel = this.repositoryViewModels.Last();
        }
    }
}