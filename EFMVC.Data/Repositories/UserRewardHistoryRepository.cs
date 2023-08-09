using EFMVC.Data.Infrastructure;
using EFMVC.Model;
using EFMVC.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Data.Repositories
{
    public interface IUserRewardHistoryRepository : IRepository<UserRewardHistory>
    {
    }
    public class UserRewardHistoryRepository : RepositoryBase<UserRewardHistory>, IUserRewardHistoryRepository
    {
        public UserRewardHistoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
