namespace ReactiveGit.Gui.Base.Model.Factories
{
    /// <summary>
    /// The default repository details factory.
    /// </summary>
    public class DefaultRepositoryDetailsFactory : IRepositoryDetailsFactory
    {
        /// <inheritdoc />
        public IRepositoryDetails CreateRepositoryDetails(string repositoryDirectory)
        {
            return new RepositoryDetails(repositoryDirectory);
        }
    }
}
