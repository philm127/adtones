using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.SearchClass
{
    public class PromotionalCampaignAuditFilter
    {
        public string PromotionalCampaignName { get; set; }
        public string PromotionalAdvertName { get; set; }
        public string MSISDN { get; set; }
        public string StartFromtime { get; set; }
        public string StartTotime { get; set; }
        public string EndFromtime { get; set; }
        public string EndTotime { get; set; }
        public string FromPlayLength { get; set; }
        public string ToPlayLength { get; set; }
        public string DTMFKey { get; set; }
    }
}