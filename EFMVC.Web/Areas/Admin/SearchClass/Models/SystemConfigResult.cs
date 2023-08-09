using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.Models
{
    public class SystemConfigResult
    {
        public int SystemConfigId { get; set; }

      
        public string SystemConfigKey { get; set; }

       
        public string SystemConfigValue { get; set; }

        public string SystemConfigType { get; set; }

        
        public DateTime CreatedDateTimeSort { get; set; }
        public string CreatedDateTime { get; set; }

        public DateTime UpdatedDateTime { get; set; }
    }
}