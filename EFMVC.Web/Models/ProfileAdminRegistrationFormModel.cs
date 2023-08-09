using System;
using System.ComponentModel.DataAnnotations;

namespace EFMVC.Web.Models
{
    public class ProfileAdminRegistrationFormModel
    {
        #region Users Data

        public int UserId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email address provided is not in the correct format.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email address provided is not in the correct format.")]
        public string Email { get; set; }

        public string Organisation { get; set; }

        //[Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public int OperatorId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime LastLoginTime { get; set; }

        public int Activated { get; set; }

        public int RoleId { get; set; }

        public bool VerificationStatus { get; set; }

        public int Outstandingdays { get; set; }

        public bool IsMsisdnMatch { get; set; }

        public bool IsEmailVerfication { get; set; }

        public string PhoneticAlphabet { get; set; }

        public bool IsMobileVerfication { get; set; }

        public int? OrganisationTypeId { get; set; }

        public string UserMatchTableName { get; set; }

        public string OldEmail { get; set; }
        public string OldPassword { get; set; }

        #endregion

        #region Contacts Data

        public int Id { get; set; }

        public int ContactUserId { get; set; }

        [Required(ErrorMessage = "The Mobile Number field is required.")]
        [DataType(DataType.Text)]
        public string MobileNumber { get; set; }

        public string FixedLine { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public int CountryId { get; set; }

        public int? CurrencyId { get; set; }

        #endregion
    }
}