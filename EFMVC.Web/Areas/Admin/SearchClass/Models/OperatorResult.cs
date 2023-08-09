﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.Models
{
    public class OperatorResult
    {
        public int OperatorId { get; set; }
        public string Name { get; set; }

        public int? CountryId { get; set; }
        public string CountryName { get; set; }

        public string IsActive { get; set; }
        public decimal EmailCost { get; set; }
        public decimal SmsCost { get; set; }
        public string Currency { get; set; }
    }
}