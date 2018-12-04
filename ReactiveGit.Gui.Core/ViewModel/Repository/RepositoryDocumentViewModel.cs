// <copyright file="RepositoryDocumentViewModel.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.Core.ViewModel.Repository
{
    using ReactiveGit.Gui.Core.Model;
    using ReactiveGit.Gui.Core.ViewModel.CommitHistory;
    using ReactiveGit.Gui.Core.ViewModel.Content;
    using ReactiveGit.Gui.Core.ViewModel.Factories;

    using ReactiveUI.Fody.Helpers;

    /// <summary>
    /// View model associated with a repository document.
    /// </summary>
    public class RepositoryDocumentViewModel : ContentViewModelBase, IRepositoryDocumentViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryDocumentViewModel" /> class.
        /// </summary>
        /// <param name="factory">A factory for creating the children.</param>
        /// <param name="repositoryDetails">The details about the repositories.</param>
        public RepositoryDocumentViewModel(IRepositoryViewModelFactory factory, IRepositoryDetails repositoryDetails)
        {
            this.RepositoryDetails = repositoryDetails;
            this.CommitHistoryViewModel = factory.CreateCommitHistoryViewModel(this.RepositoryDetails);
        }

        /// <inheritdoc />
        public ICommitHistoryViewModel CommitHistoryViewModel { get; }

        /// <inheritdoc />
        public override string FriendlyName => this.RepositoryDetails.FriendlyName;

        /// <inheritdoc />
        [Reactive]
        public IRepositoryDetails RepositoryDetails { get; set; }

        /// <inheritdoc />
        public string RepositoryPath => this.RepositoryDetails.RepositoryManager.RepositoryPath;
    }
}