// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveGit.Gui.Core.Model;
using ReactiveGit.Gui.Core.ViewModel.CommitHistory;
using ReactiveGit.Gui.Core.ViewModel.Content;
using ReactiveGit.Gui.Core.ViewModel.Factories;

using ReactiveUI.Fody.Helpers;

namespace ReactiveGit.Gui.Core.ViewModel.Repository
{
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
            RepositoryDetails = repositoryDetails;
            CommitHistoryViewModel = factory.CreateCommitHistoryViewModel(RepositoryDetails);
        }

        /// <inheritdoc />
        public ICommitHistoryViewModel CommitHistoryViewModel { get; }

        /// <inheritdoc />
        public override string FriendlyName => RepositoryDetails.FriendlyName;

        /// <inheritdoc />
        [Reactive]
        public IRepositoryDetails RepositoryDetails { get; set; }

        /// <inheritdoc />
        public string RepositoryPath => RepositoryDetails.RepositoryManager.RepositoryPath;
    }
}