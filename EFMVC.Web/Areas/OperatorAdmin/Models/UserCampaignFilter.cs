using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.OperatorAdmin.Models
{
    public class UserCampaignFilter
    {
        public string FromTotalBudget { get; set; }
        public string ToTotalBudget { get; set; }
        public string FromTotalAverageBid { get; set; }
        public string ToTotalAverageBid { get; set; }
        public string FromTotalSpend { get; set; }
        public string ToTotalSpend { get; set; }
        public string Fromdate { get; set; }
        public string Todate { get; set; }
    }
}