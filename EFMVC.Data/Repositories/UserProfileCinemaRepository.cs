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
    /// <seealso cref="EFMVC.Data.Infrastructure.IRepository{EFMVC.Model.UserProfileCinema}" />
    public interface IUserProfileCinemaRepository : IRepository<UserProfileCinema>
    {
    }
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EFMVC.Data.Infrastructure.RepositoryBase{EFMVC.Model.UserProfileCinema}" />
    /// <seealso cref="EFMVC.Data.Repositories.IUserProfileCinemaRepository" />
    public class UserProfileCinemaRepository : RepositoryBase<UserProfileCinema>, IUserProfileCinemaRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdvertRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public UserProfileCinemaRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    
}
