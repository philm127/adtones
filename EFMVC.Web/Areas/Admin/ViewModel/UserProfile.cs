using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.ViewModel
{
    public class UserProfile
    {
        public int Id { get; set; }
        [Required]
        public int RoleId { get; set; }
        public string Email { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Organisation { get; set; }
        [Required]
        public int Outstandingdays { get; set; }

        

    }
}