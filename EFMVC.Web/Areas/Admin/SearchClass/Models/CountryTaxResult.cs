using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.Models
{
    public class CountryTaxResult
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }

        public string ShortName { get; set; }

        public decimal TaxPercantage { get; set; }

        public DateTime? CreatedDateSort { get; set; }
        public string CreatedDate { get; set; }
        public int Status { get; set; }
    }
}