using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.ViewModels
{
    public class ContactsFormModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        
        public string MobileNumber { get; set; }


        public string FixedLine { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }


        public string Address { get; set; }

        public int? CountryId { get; set; }

        public int? CurrencyId { get; set; }
    }
}