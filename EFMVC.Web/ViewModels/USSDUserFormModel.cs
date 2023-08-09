using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EFMVC.Web.ViewModels
{
    public class USSDUserFormModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email address provided is not in the correct format.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email address provided is not in the correct format.")]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string VerifcationCode { get; set; }

        [Display(Name = "MobileNumber")]
        public string MSISDN { get; set; }
    }
}