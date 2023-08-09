using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.ProfileAdmin.Models
{
    public class UserCampaignResult
    {
        public int CampaignProfileId { get; set; }
        public string CampaignName { get; set; }
        public int? AdvertId { get; set; }
        public string AdvertName { get; set; }
        public int TotalPlayCount { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public int? OperatorId { get; set; }
        public string OperatorName { get; set; }
        public string ProfileType { get; set; }
        public string ProfileName { get; set; }
        public string ExpectedResponse { get; set; }
        public string Listen { get; set; }
    }
}