namespace ReactiveGit.Gui.Base.ViewModel
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using ReactiveGit.Gui.Base.ViewModel.Repository;

    using ReactiveUI;

    /// <summary>
    /// The main view model where we host our repositories and other details.
    /// </summary>
    public interface IMainViewModel : IReactiveObject
    {
        /// <summary>
        /// Gets a command that will select the repository.
        /// </summary>
        ICommand SelectRepository { get; }

        /// <summary>
        /// Gets a command which will create a new repository.
        /// </summary>
        ICommand NewRepository { get; }

        /// <summary>
        /// Gets a list of repository view models.
        /// </summary>
        IReadOnlyList<IRepositoryDocumentViewModel> RepositoryViewModels { get; }

        /// <summary>
        /// Gets or sets the selected repository.
        /// </summary>
        IRepositoryDocumentViewModel SelectedRepositoryViewModel { get; set; }
    }
}