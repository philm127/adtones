using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.SearchClass.Models
{
    public class ProfileInformationResult
    {
        public int Id { get; set; }
        public string ProfileName { get; set; }

        public int CountryId { get; set; }
        public string CountryName { get; set; }

        public string ProfileType { get; set; }

        public bool Status { get; set; }
    }
}