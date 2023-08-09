using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model {
    
    public class CampaignProfileTimeSetting {
        [Key]
        public int CampaignProfileTimeSettingsId { get; set; }
        public int CampaignProfileId { get; set; }
        public CampaignProfile CampaignProfile { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string Sunday { get; set; }
        public int? AdtoneServerCampaignProfileTimeId { get; set; }
    }
}
