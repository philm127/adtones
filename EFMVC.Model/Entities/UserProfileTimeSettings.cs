using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model {
    
    public class UserProfileTimeSetting {
        [Key]
        public int UserProfileTimeSettingsId { get; set; }
        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string Sunday { get; set; }
        public int? AdtoneServerUserProfileTimeSettingId { get; set; }
    }
}
