using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.ViewModel
{
    public class UserCreditFormModel
    {
        [Key]
        public int Id { get; set; }
        public int CountryId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        [Required]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid credit amount")]
        public decimal  AssignCredit { get; set; }
    }
}