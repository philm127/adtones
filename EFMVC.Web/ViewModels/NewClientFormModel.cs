using EFMVC.Web.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.ViewModels
{
    public class NewClientFormModel
    {
        [Key]
        public int ClientId { get; set; }

        public int UserId { get; set; }

        [Required]
        [ExcludeChar("/.,!@#$%", ErrorMessage = "Name contains invalid character.")]
        public string ClientName { get; set; }

        //[Required]
        [MaxLength(300, ErrorMessage = "Maximum Length Exceeded. Client Description cannot be more than 300 charaters")]
        public string ClientDescription { get; set; }

        public string ClientContactInfo { get; set; }

        public decimal ClientBudget { get; set; }

        [Required]
        [EmailAddress]
        public string ClientEmail { get; set; }

        public string ClientPhoneticAlphabet { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int ClientStatus { get; set; }

        public bool NextStatus { get; set; }

        [Required]
        public string ClientContactPhone { get; set; }

        public int? CountryId { get; set; }

        public int? AdtoneServerClientId { get; set; }
    }
}