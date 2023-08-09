using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.SearchClass
{
    public class CampaignCreditFilter
    {
        public int CampaignCreditPeriodId { get; set; }
        public int UserId { get; set; }
        public int CampaignProfileId { get; set; }
    }
}