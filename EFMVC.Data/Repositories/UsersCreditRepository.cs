using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.Data.Infrastructure;
using EFMVC.Model;

namespace EFMVC.Data.Repositories
{
    /// <summary>
    /// Interface IUsersCreditRepository
    /// </summary>
    /// <seealso cref="EFMVC.Data.Infrastructure.IRepository{EFMVC.Model.UsersCredit}" />
    public interface IUsersCreditRepository : IRepository<UsersCredit>
    {
    }
    public  class UsersCreditRepository : RepositoryBase<UsersCredit>, IUsersCreditRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersCreditRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public UsersCreditRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
