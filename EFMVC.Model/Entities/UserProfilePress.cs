using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model {
    
    public class UserProfilePress {
        [Key]
        public int UserProfilePressId { get; set; }
        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
        public int? Local { get; set; }
        public int? National { get; set; }
        public int? FreeNewpapers { get; set; }
        public int? Magazines { get; set; }
    }
}
