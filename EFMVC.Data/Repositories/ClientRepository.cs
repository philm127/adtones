using EFMVC.Data.Infrastructure;
using EFMVC.Model;

namespace EFMVC.Data.Repositories
{
    public class ClientRepository : RepositoryBase<Client>, IClientRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public ClientRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    /// <summary>
    /// Interface IUserRepository
    /// </summary>
    public interface IClientRepository : IRepository<Client>
    {
    }
}
