using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.Data.Infrastructure;
using EFMVC.Model;
using EFMVC.Model.Entities;

namespace EFMVC.Data.Repositories
{
    /// <summary>
    /// Interface ICampaignCreditPeriodRepository
    /// </summary>
    /// <seealso cref="EFMVC.Data.Infrastructure.IRepository{EFMVC.Model.CampaignCreditPeriod}" />
    public interface ICampaignCreditPeriodRepository : IRepository<CampaignCreditPeriod>
    {
    }
    public  class CampaignCreditPeriodRepository : RepositoryBase<CampaignCreditPeriod>, ICampaignCreditPeriodRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignCreditPeriodRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public CampaignCreditPeriodRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
