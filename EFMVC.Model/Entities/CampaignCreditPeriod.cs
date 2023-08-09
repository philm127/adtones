using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model.Entities
{
    public class CampaignCreditPeriod
    {
        public int CampaignCreditPeriodId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int CampaignProfileId { get; set; }
        public virtual CampaignProfile CampaignProfile { get; set; }
        public int CreditPeriod { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? AdtoneServerCampaignCreditPeriodId { get; set; }
    }
}
