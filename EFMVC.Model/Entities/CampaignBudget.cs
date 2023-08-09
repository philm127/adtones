using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model.Entities
{
    public class CampaignBudget
    {
        public int CampaignBudgetId { get; set; }
        public int CampaignProfileId { get; set; }
        public decimal ProvidendSpendAmount { get; set; }
        public int BucketCount { get; set; }
        public decimal AvailableBudget { get; set; }
    }
}
