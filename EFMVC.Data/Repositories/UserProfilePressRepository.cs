using EFMVC.Data.Infrastructure;
using EFMVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Data.Repositories
{
    public interface IUserProfilePressRepository : IRepository<UserProfilePress>
    {
    }
    public class UserProfilePressRepository : RepositoryBase<UserProfilePress>, IUserProfilePressRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdvertRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public UserProfilePressRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
  
}
