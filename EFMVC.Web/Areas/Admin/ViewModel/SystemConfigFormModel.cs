using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.Areas.Admin.ViewModel
{
    public class SystemConfigFormModel
    {
        [Key]
        public int SystemConfigId { get; set; }
        [Required]
        public string SystemConfigKey { get; set; }

        [Required]
        public string SystemConfigValue { get; set; }
        [Required]
        public string SystemConfigType{ get; set; }
        public IEnumerable<SelectListItem> GetSystemConfigType
        {
            get
            {
                return new[]
                {
                new SelectListItem { Value = "Website", Text = "Website" },
                new SelectListItem { Value = "ProvisioningService", Text = "ProvisioningService" },

            };
            }
        }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        /// <value>The created date time.</value>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the updated date time.
        /// </summary>
        /// <value>The updated date time.</value>
        public DateTime UpdatedDateTime { get; set; }


    }
}