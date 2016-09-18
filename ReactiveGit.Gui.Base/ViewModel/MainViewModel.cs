// <copyright file="MainViewModel.cs" company="Glenn Watson">
// Copyright (c) Glenn Watson. All rights reserved.
// </copyright>
namespace ReactiveGit.Gui.Base.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;
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

        private readonly IRepositoryDetailsFactory repositoryFactory;

        private readonly ReactiveList<IRepositoryDocumentViewModel> repositoryViewModels;

        private readonly ReactiveCommand<Unit, string> selectRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="folderSelector">A class for generating folders.</param>
        /// <param name="repositoryFactory">Factory for creating the repository details.</param>
        /// <param name="viewModelFactory">Factory for creating the document view models.</param>
        public MainViewModel(IFolderSelector folderSelector, IRepositoryDetailsFactory repositoryFactory, IRepositoryViewModelFactory viewModelFactory)
        {
            this.repositoryFactory = repositoryFactory;
            this.folderSelector = folderSelector;

            this.selectRepository = ReactiveCommand.CreateFromObservable(() => this.folderSelector.Prompt());
            this.selectRepository.Subscribe(x =>
                {
                    var newModel = repositoryFactory.CreateRepositoryDetails(x);
                    this.repositoryViewModels.Add(viewModelFactory.CreateRepositoryViewModel(newModel));
                    this.SelectedRepositoryViewModel = this.repositoryViewModels.Last();
                });

            this.repositoryViewModels = new ReactiveList<IRepositoryDocumentViewModel>();
        }

        /// <summary>
        /// Gets a command that will select the repository.
        /// </summary>
        public ICommand SelectRepository => this.selectRepository;

        /// <inheritdoc />
        public IReadOnlyList<IRepositoryDocumentViewModel> RepositoryViewModels => this.repositoryViewModels;

        /// <summary>
        /// Gets or sets the selected view model.
        /// </summary>
        [Reactive]
        public IRepositoryDocumentViewModel SelectedRepositoryViewModel { get; set; }
    }
}