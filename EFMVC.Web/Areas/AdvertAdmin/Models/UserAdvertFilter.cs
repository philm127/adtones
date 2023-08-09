using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.AdvertAdmin.Models
{
    public class UserAdvertFilter
    {
        public string Fromdate { get; set; }
        public string Todate { get; set; }
        //public DateTime? Fromdate { get; set; }
        //public DateTime? Todate { get; set; }
    }
}