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
    public interface IRewardRepository : IRepository<Reward>
    {
    }
    public class RewardRepository : RepositoryBase<Reward>, IRewardRepository
    {
        public RewardRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
