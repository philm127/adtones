using System;
using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model
{
    public class RotateAdvert
    {
        [Key]
        public int RotateAdvertId { get; set; }
        public Nullable<int> UserProfileId { get; set; }
        public Nullable<int> CampaignProfileId { get; set; }
        public Nullable<int> AdvertId { get; set; }
        public bool IsAdvertPlayed { get; set; }
        public string UserMatchTableName { get; set; }
        public DateTime AddedDate { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public virtual CampaignProfile CampaignProfile { get; set; }
        public virtual Advert Advert { get; set; }
    }
}
