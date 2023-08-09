using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Users.Models
{
    public class ConfirmCodeModel
    {
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public string ConnectionToken { get; set; }
        public string UserToken { get; set; }

        //[Required(ErrorMessage = "Please enter the verification code")]
        //[MinLength(8,ErrorMessage = "Please enter 6 digit verification code")]
        public int UserId { get; set; }
        public string Confirm { get; set; }
    }
}