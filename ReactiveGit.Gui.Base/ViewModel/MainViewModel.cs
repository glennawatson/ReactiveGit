// <copyright file="MainViewModel.cs" company="Glenn Watson">
// Copyright (c) Glenn Watson. All rights reserved.
// </copyright>

namespace ReactiveGit.Gui.Base.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Reactive;
    using System.Windows.Input;

    using ReactiveGit.Gui.Base.View;

    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;

    /// <summary>
    /// The main view model.
    /// </summary>
    public class MainViewModel : ReactiveObject, IMainViewModel
    {
        private readonly IFolderSelector folderSelector;

        private readonly ReactiveCommand<Unit, string> selectRepository;

        private readonly ReactiveList<IRepositoryViewModel> repositoryViewModels;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="folderSelector">A class for generating folders.</param>
        public MainViewModel(IFolderSelector folderSelector)
        {
            this.folderSelector = folderSelector;

            this.selectRepository = ReactiveCommand.CreateFromObservable(() => this.folderSelector.Prompt());
            this.selectRepository.Subscribe(x =>
                {
                    var newModel = new RepositoryViewModel(x);
                    this.repositoryViewModels.Add(newModel);
                    this.SelectedRepositoryViewModel = newModel;
                });

            this.repositoryViewModels = new ReactiveList<IRepositoryViewModel>();
        }

        /// <summary>
        /// Gets a command that will select the repository.
        /// </summary>
        public ICommand SelectRepository => this.selectRepository;

        /// <inheritdoc />
        public IReadOnlyList<IRepositoryViewModel> RepositoryViewModels => this.repositoryViewModels;

        [Reactive]
        public IRepositoryViewModel SelectedRepositoryViewModel { get; set; }
    }
}
