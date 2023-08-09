using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.ViewModels
{
    public class FilterCritearea
    {
        public string ClientId { get; set; }
        public string CampaignName { get; set; }
        public DateTime ? Fromdate { get; set; }
        public DateTime ? Todate { get; set; }
        public String Status { get; set; }
        public string  FromSpend { get; set; }
        public string ToSpend { get; set; }
        public string  FromPlays { get; set; }
        public string ToPlays { get; set; }
        public string FromAvgbid { get; set; }
        public string ToAvgbid { get; set; }

    }
}