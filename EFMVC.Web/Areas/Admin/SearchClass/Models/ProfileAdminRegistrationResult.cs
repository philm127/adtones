using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.Models
{
    public class ProfileAdminRegistrationResult
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? CreatedDateSort { get; set; }
        public string CreatedDate { get; set; }
        public string IsActive { get; set; }
    }
}