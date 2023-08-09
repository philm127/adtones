using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model
{
    public class CampaignProfileUserProfile
    {
        [Key]
        public int Id { get; set; }
        public int CampaignProfileId { get; set; }
        public int UserProfileId { get; set; }

        public virtual CampaignProfile CampaignProfile { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}
