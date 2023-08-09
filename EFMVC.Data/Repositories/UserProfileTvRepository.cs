using EFMVC.Data.Infrastructure;
using EFMVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Data.Repositories
{
    public interface IUserProfileTvRepository : IRepository<UserProfileTv>
    {
    }
    public class UserProfileTvRepository : RepositoryBase<UserProfileTv>, IUserProfileTvRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdvertRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public UserProfileTvRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
 
}
