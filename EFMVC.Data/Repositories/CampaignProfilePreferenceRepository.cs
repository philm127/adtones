using EFMVC.Data.Infrastructure;
using EFMVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Data.Repositories
{
    public interface ICampaignProfilePreferenceRepository : IRepository<CampaignProfilePreference>
    {
    }
    public class CampaignProfilePreferenceRepository : RepositoryBase<CampaignProfilePreference>, ICampaignProfilePreferenceRepository
    {
        public CampaignProfilePreferenceRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
