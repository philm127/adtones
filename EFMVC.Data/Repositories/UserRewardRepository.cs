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
    public interface IUserRewardRepository : IRepository<UserReward>
    {
    }
    public class UserRewardRepository : RepositoryBase<UserReward>, IUserRewardRepository
    {
        public UserRewardRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
