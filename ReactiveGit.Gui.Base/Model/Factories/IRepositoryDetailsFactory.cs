namespace ReactiveGit.Gui.Base.Model.Factories
{
    /// <summary>
    /// Factory for generating 
    /// </summary>
    public interface IRepositoryDetailsFactory
    {
        /// <summary>
        /// Creates a new repository for the specified repository directory.
        /// </summary>
        /// <param name="repositoryDirectory">The path to the repository.</param>
        /// <returns>The repository details.</returns>
        IRepositoryDetails CreateRepositoryDetails(string repositoryDirectory);
    }
}