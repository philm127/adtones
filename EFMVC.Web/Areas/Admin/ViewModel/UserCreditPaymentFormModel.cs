using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.ViewModel
{
    public class UserCreditPaymentFormModel
    {
        [Required]
        public int UserId { get; set; }
        [Required(ErrorMessage ="Please select atleaset one Invoice No.")]
        public int BillingId { get; set; }

        [Required]       
        [Range(1, Int64.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid amount")]
        public decimal Amount { get; set; }
        [Required]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please select atleaset one campaign")]
        public int CampaignProfileId { get; set; }
    }
}