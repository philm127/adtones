using EFMVC.Data.Infrastructure;
using EFMVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Data.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EFMVC.Data.Infrastructure.IRepository{EFMVC.Model.UserProfile}" />
    public interface IUserProfileRepository : IRepository<UserProfile>
    {
    }
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EFMVC.Data.Infrastructure.RepositoryBase{EFMVC.Model.UserProfile}" />
    /// <seealso cref="EFMVC.Data.Repositories.IUserProfileRepository" />
    public class UserProfileRepository : RepositoryBase<UserProfile>, IUserProfileRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdvertRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public UserProfileRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
   
}
