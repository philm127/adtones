using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Models
{
    public class AdvertResult
    {
        public int AdvertId { get; set; }
        public int? ClientId { get; set; }
        public string ClientName { get; set; }
        public string Name { get; set; }
        public int NoOfCompaign { get; set; }

        public int CampaignId { get; set; }

        public DateTime CreatedDateSort { get; set; }
        public string CreatedDate { get; set; }

        public string MediaFileLocation { get; set; }

        public string Status { get; set; }

        public int fStatus { get; set; }

        public decimal TotalPlays { get; set; }

        public decimal AvgBid { get; set; }

        public bool IsAdminApproval { get; set; }
    }
}