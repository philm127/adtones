using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model {   
    public class CampaignProfileInternet {
        [Key]
        public int CampaignProfileInternetId { get; set; }
        public int CampaignProfileId { get; set; }
        public CampaignProfile CampaignProfile { get; set; }

        [StringLength(50)]
        public string SocialNetworking { get; set; }

        [StringLength(50)]
        public string Video { get; set; }

        [StringLength(50)]
        public string Research { get; set; }

        [StringLength(50)]
        public string Auctions { get; set; }

        [StringLength(50)]
        public string Shopping { get; set; }
    }
}
