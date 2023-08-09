using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EFMVC.Web.ViewModels
{
    public class AreaFormModel
    {
        public int AreaId { get; set; }

        [Required]
        public string AreaName { get; set; }

        public bool IsActive { get; set; }
        [Required]
        public int CountryId { get; set; }

    }
}