using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.SearchClass.Models
{
    public class PromotionalCampaignAuditResult
    {
        public int PromotionalCampaignAuditId { get; set; }
        public string OperatorName { get; set; }
        public string PromotionalCampaignName { get; set; }
        public string PromotionalAdvertName { get; set; }
        public string MSISDN { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string DisplayStartTime { get; set; }
        public string DisplayEndTime { get; set; }
        public double PlayLengthTicks { get; set; }
        public string DTMFKey { get; set; }
    }
}