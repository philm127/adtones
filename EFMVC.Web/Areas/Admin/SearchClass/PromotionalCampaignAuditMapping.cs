using EFMVC.Web.Areas.Admin.SearchClass.Models;
using EFMVC.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.SearchClass
{
    public class PromotionalCampaignAuditMapping
    {
        public PromotionalCampaignAuditFilter promotionalCampaignAuditFilter { get; set; }
        public List<PromotionalCampaignAuditResult> promotionalCampaignAuditResult { get; set; }
        public PromotionalCampaignAuditDashboardResult promotionalCampaignAuditDashboardResult { get; set; }
    }
}