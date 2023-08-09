using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model {
    public class CampaignProfileMobile {
        [Key]
        public int CampaignProfileMobileId { get; set; }
        public int CampaignProfileId { get; set; }
        public CampaignProfile CampaignProfile { get; set; }
        public string ContractType { get; set; }
        public string Spend { get; set; }
    }
}
