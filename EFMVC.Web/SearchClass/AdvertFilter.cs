using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.SearchClass
{
    public class AdvertFilter
    {
        public int AdvertId { get; set; }
        public string ClienId { get; set; }
        public string ClientName { get; set; }

        public string Name { get; set; }

        //public DateTime? Fromdate { get; set; }
        //public DateTime? Todate { get; set; }
        public string Fromdate { get; set; }
        public string Todate { get; set; }

        public string FromPlays { get; set; }
        public string ToPlays { get; set; }

        public string Frombid { get; set; }
        public string Tobid { get; set; }

        public String Status { get; set; }
    }
}