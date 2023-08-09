using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.Models
{
    public class AreaResult
    {
        public int AreaId { get; set; }
        public string Name { get; set; }

        public int? CountryId { get; set; }
        public string CountryName { get; set; }
    }
}