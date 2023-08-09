using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.ViewModel
{
    public class CampaignCreditFormModel
    {
        public int CampaignCreditPeriodId { get; set; }
        [Required(ErrorMessage = "Please select atleaset one advertiser.")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Please select atleaset one campaign.")]
        public int CampaignProfileId { get; set; }
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Credit Period must be a natural number")]
        [Required(ErrorMessage = "The Credit period is required.")]
        public int CreditPeriod { get; set; } = 7;
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? AdtoneServerCampaignCreditPeriodId { get; set; }
    }
}