using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.OperatorAdmin.Models
{
    public class UserCampaignResult
    {
        public int CampaignProfileId { get; set; }
        public string CampaignName { get; set; }

        public int? ClientId { get; set; }
        public string ClientName { get; set; }

        public int UserId { get; set; }
        public string AdvertiserCompanyName { get; set; }

        public string AdvertiserMobileNumber { get; set; }

        public decimal TotalBudget { get; set; }
        public decimal TotalAverageBid { get; set; }
        public decimal TotalSpend { get; set; }

        public string Status { get; set; }

        public string CurrencyCode { get; set; }
        public int? CountryId { get; set; }
        public int? CurrencyId { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public string DisplayCreatedDateTime { get; set; }
    }
}