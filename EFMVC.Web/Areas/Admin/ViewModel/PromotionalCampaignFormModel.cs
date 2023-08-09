using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.ViewModel
{
    public class PromotionalCampaignFormModel
    {
        public int ID { get; set; }
        [Required]
        public int CountryId { get; set; }
        [Required]
        public int OperatorId { get; set; }
        [Required]
        public string CampaignName { get; set; }
        [Required]
        public int BatchID { get; set; }
        [Required]
        public string MaxDaily { get; set; }
        [Required]
        public string MaxWeekly { get; set; }
        [Required]
        public string AdvertName { get; set; }
        public string AdvertLocation { get; set; }
        public int Status { get; set; }
    }
}