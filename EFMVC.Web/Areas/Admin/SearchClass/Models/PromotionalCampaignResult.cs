using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.Models
{
    public class PromotionalCampaignResult
    {
        public int CampaignId { get; set; }
        public int? OperatorId { get; set; }
        public string CampaignName { get; set; }
        public string OperatorName { get; set; }
        public int BatchID { get; set; }
        public int MaxDaily { get; set; }
        public int MaxWeekly { get; set; }
        public string AdvertName { get; set; }
        public string AdvertLocation { get; set; }
        public int Status { get; set; }
        public string rStatus { get; set; }
    }
}