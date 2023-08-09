using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.Models
{
    public class AdvertiserSocialLoginModel
    {
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email address provided is not in the correct format.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email address provided is not in the correct format.")]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        public string Organisation { get; set; }
        public int?  OrganisationTypeId { get; set; }


        [Required(ErrorMessage = "The Contact number field is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Contact number")]
        //[StringLength(10, ErrorMessage = "The Contact number must be at least 10 characters long.", MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        public int OperatorId { get; set; }

        [Required(ErrorMessage = "The Country field is required.")]
        public int CountryId { get; set; }
        public string CountryCode { get; set; }

        public string opratorAdminId { get; set; }
        public List<SelectListItem> opratorAdminList { get; set; }

    }
}