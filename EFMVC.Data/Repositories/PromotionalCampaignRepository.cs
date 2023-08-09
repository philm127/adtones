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
    public interface IPromotionalCampaignRepository : IRepository<PromotionalCampaign>
    {
    }
    public class PromotionalCampaignRepository : RepositoryBase<PromotionalCampaign>, IPromotionalCampaignRepository
    {
        public PromotionalCampaignRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
