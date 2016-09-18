namespace ReactiveGit.Gui.Base.Model.Factories
{
    using ReactiveGit.Managers;

    /// <summary>
    /// The default repository details factory.
    /// </summary>
    public class DefaultRepositoryFactory : IRepositoryFactory
    {
        /// <inheritdoc />
        public IRepositoryDetails CreateRepositoryDetails(string repositoryDirectory)
        {
            return new RepositoryDetails(repositoryDirectory);
        }

        /// <inheritdoc />
        public IRepositoryCreator CreateRepositoryCreator()
        {
            return new RepositoryCreator(x => new GitProcessManager(x, null));
        }
    }
}