﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.Models
{
    public class AdvertCategoryResult
    {
        public int AdvertCategoryId { get; set; }
        public string Name { get; set; }

        public int? CountryId { get; set; }
        public string CountryName { get; set; }

        public DateTime? CreatedDateSort { get; set; }
        public string CreatedDate { get; set; }
    }
}