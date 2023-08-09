using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Models
{
    public class Campaignchartresult
    {
        public int status { get; set; }
        public NoOfPlayCampaign[] _playresult { get; set; }
        public NoOfAvgbidCampaign[] _Avgresult { get; set; }
    }
}