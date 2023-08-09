using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model
{
    public class CampaignAdvert
    {
        public CampaignAdvert() { }

        [Key]
        public int CampaignAdvertId { get; set; }
        public int CampaignProfileId { get; set; }
        public int AdvertId { get; set; }

        public bool NextStatus { get; set; }
        public int? AdtoneServerCampaignAdvertId { get; set; }
        public virtual Advert Advert { get; set; }
        public virtual CampaignProfile CampaignProfile { get; set; }
    }
}
