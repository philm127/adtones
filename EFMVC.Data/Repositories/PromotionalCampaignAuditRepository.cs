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
    public interface IPromotionalCampaignAuditRepository : IRepository<PromotionalCampaignAudit>
    {
    }
    public class PromotionalCampaignAuditRepository : RepositoryBase<PromotionalCampaignAudit>, IPromotionalCampaignAuditRepository
    {
        public PromotionalCampaignAuditRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
