using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.SearchClass.Models
{
    public class PromotionalCampaignAuditDashboardResult
    {
        public double PlaystoDate { get; set; }
        public double AveragePlayTime { get; set; }
        public decimal MaxPlayLength { get; set; }
        public double MaxPlayLengthPercantage {get;set;}
        public int TotalReach { get; set; }
    }
}