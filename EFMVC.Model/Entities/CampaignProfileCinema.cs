using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model {
    public class CampaignProfileCinema {
        [Key]
        public int CampaignProfileCinemaId { get; set; }
        public int CampaignProfileId { get; set; }
        public CampaignProfile CampaignProfile { get; set; }

        [StringLength(50)]
        public string Cinema { get; set; }
    }
}
