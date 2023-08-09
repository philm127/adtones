using System;

namespace EFMVC.Web.Areas.Admin.SearchClass
{
    public class SystemConfigFilter
    {
        public string SystemConfigKey { get; set; }


        public string SystemConfigValue { get; set; }

        public string SystemConfigType { get; set; }
        //public DateTime? Fromdate { get; set; }
        //public DateTime? Todate { get; set; }
        public string Fromdate { get; set; }
        public string Todate { get; set; }
    }
}