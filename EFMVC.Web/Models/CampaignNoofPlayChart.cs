using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Models
{
    public class CampaignNoofPlayChart
    {
        public int NoofplayMaxCount { get; set; }
        public int AvgbidMaxCount { get; set; }
        public List<Campaignchartresult> _Campaignavgplayresult { get; set; }

        public CampaignNoofPlayChart()
        {
            _Campaignavgplayresult = new List<Campaignchartresult>();
        }
    }
}