using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.Models
{
    public class OperatorRegistrationResult
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Organisation { get; set; }
        public DateTime? CreatedDateSort { get; set; }
        public string CreatedDate { get; set; }
        public string IsActive { get; set; }

        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int OperatorId { get; set; }
        public string OperatorName { get; set; }
    }
}