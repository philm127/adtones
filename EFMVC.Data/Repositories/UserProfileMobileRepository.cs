using EFMVC.Data.Infrastructure;
using EFMVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Data.Repositories
{
    public interface IUserProfileMobileRepository : IRepository<UserProfileMobile>
    {
    }
    public class UserProfileMobileRepository : RepositoryBase<UserProfileMobile>, IUserProfileMobileRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdvertRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public UserProfileMobileRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
   
}
