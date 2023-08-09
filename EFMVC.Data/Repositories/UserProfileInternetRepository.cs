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
    /// <seealso cref="EFMVC.Data.Infrastructure.IRepository{EFMVC.Model.UserProfileInternet}" />
    public interface IUserProfileInternetRepository : IRepository<UserProfileInternet>
    {
    }
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EFMVC.Data.Infrastructure.RepositoryBase{EFMVC.Model.UserProfileInternet}" />
    /// <seealso cref="EFMVC.Data.Repositories.IUserProfileInternetRepository" />
    public class UserProfileInternetRepository : RepositoryBase<UserProfileInternet>, IUserProfileInternetRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdvertRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public UserProfileInternetRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
   
}
