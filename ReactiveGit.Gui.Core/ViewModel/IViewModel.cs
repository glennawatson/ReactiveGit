namespace ReactiveGit.Gui.Core.ViewModel
{
    using System.Windows.Input;

    using ReactiveGit.Gui.Core.Model;

    /// <summary>
    /// A view model in the reactive git program.
    /// </summary>
    public interface IViewModel
    {
        /// <summary>
        /// Gets or sets the repository details.
        /// </summary>
        IRepositoryDetails RepositoryDetails { get; set; }
    }
}
