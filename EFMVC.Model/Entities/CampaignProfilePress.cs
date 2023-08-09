using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model {
    
    public class CampaignProfilePress {
        [Key]
        public int CampaignProfilePressId { get; set; }
        public int CampaignProfileId { get; set; }
        public CampaignProfile CampaignProfile { get; set; }

        [StringLength(50)]
        public string Local { get; set; }

        [StringLength(50)]
        public string National { get; set; }

        [StringLength(50)]
        public string FreeNewpapers { get; set; }

        [StringLength(50)]
        public string Magazines { get; set; }
    }
}
