using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EFMVC.Web.ViewModels
{
    public class UserFormModel
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

      //  [Required]
        public string Organisation { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        // Mobile number must start with a zero and then be followed by 10 digits.
        // The country code without the + will be added after and the leading zero 
        // stripped.
        //Ex- 0123456789123
        //[RegularExpression(@"^(0[\d]{12,12})$", ErrorMessage = "Please enter a valid mobile phone number.")]
        //[RegularExpression(@"^([\d]{12,12})$", ErrorMessage = "Please enter a valid mobile phone number.")]
        [Required(ErrorMessage = "The Contact number field is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Contact Number")]
        //[StringLength(10, ErrorMessage = "The Contact number must be at least 10 characters long.", MinimumLength = 10)]
        public string MSISDN { get; set; }

        public CampaignProfileFormModel CampaignProfile { get; set; }

        public UserProfileFormModel UserProfile { get; set; }

       // [Required]
        public int? OperatorId { get; set; }

        [Required(ErrorMessage = "The Country field is required.")]
        public string CountryCode { get; set; }

        public bool IsMobileVerfication { get; set; }

        public int? OrganisationTypeId { get; set; }

        public string UserMatchTableName { get; set; }

        public bool VerificationStatus { get; set; }

        public bool IsMsisdnMatch { get; set; }

        public string VerifcationCode { get; set; }

        public string TibcoMessageId { get; set; }
        public List<SelectListItem> CountryList { get; set; }

        //[Required(ErrorMessage = "The OpratorAdmin field is required.")]
        //public string opratorAdminId { get; set; }
        public List<SelectListItem> opratorAdminList { get; set; }
        public bool IsSessionFlag { get; set; } = false;
        public Nullable<DateTime> LockOutTime { get; set; }
        public DateTime LastPasswordChangedDate { get; set; } = DateTime.Now;
    }
}