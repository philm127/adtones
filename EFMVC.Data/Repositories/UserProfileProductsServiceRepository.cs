using EFMVC.Data.Infrastructure;
using EFMVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Data.Repositories
{
    public interface IUserProfileProductsServiceRepository : IRepository<UserProfileProductsService>
    {
    }
    public class UserProfileProductsServiceRepository : RepositoryBase<UserProfileProductsService>, IUserProfileProductsServiceRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdvertRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public UserProfileProductsServiceRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    
}
