using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model.Entities
{
    public class FlotChartMetrics
    {
        public int FlotChartMetricsId { get; set; }
        public int UserId { get; set; }
        public int NoofPlayName { get; set; }
        public int NoofPlayValue { get; set; }
        public int AvgBidName { get; set; }
        public int AvgBidValue { get; set; }
        public int NoofplayMaxCount { get; set; }
        public int AvgbidMaxCount { get; set; }
        public int Status { get; set; }
    }
}
