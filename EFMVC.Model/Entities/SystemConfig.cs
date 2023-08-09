using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model
{
    public class SystemConfig
    {
        public SystemConfig() { }

        [Key]
        [Display(Name = "ID")]
        public int SystemConfigId { get; set; }

        [Display(Name = "System Config Key")]
        public string SystemConfigKey { get; set; }

        [Display(Name = "System Config Value")]
        public string SystemConfigValue { get; set; }

        [Display(Name = "Created Date/Time")]
        public DateTime CreatedDateTime { get; set; }

        [Display(Name = "Updated Date/Time")]
        public DateTime UpdatedDateTime { get; set; }

        [Display(Name = "System Config Type")]
        public string SystemConfigType { get; set; }
        
    }
}
