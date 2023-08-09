using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.ViewModels
{
    public class NewCampaignDateandInteraction
    {
        [Display(Name = "StartDate")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "EndDate")]
        public DateTime? EndDate { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "Maximum Length Exceeded. SMS Originator are limited to 50 characters")]
        [Display(Name = "SMS Originator")]
        public string SmsOriginator { get; set; }

        [MaxLength(160, ErrorMessage = "Maximum Length Exceeded. SMS Messages are limited to 160 characters")]
        [Display(Name = "SMS Text")]
        public string SmsBody { get; set; }

        [Display(Name = "Email Subject")]
        public string EmailSubject { get; set; }

        [AllowHtml]
        [Display(Name = "Email Body Text")]
        [DataType(DataType.MultilineText)]
        public string EmailBody { get; set; }
    }
}