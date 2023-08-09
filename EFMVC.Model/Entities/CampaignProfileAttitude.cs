using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model {    
    public class CampaignProfileAttitude {
        [Key]
        public int CampaignProfileAttitudeId { get; set; }
        public int CampaignProfileId { get; set; }
        public CampaignProfile CampaignProfile { get; set; }

        [StringLength(50)]
        public string Fitness { get; set; }

        [StringLength(50)]
        public string Holidays { get; set; }

        [StringLength(50)]
        public string Environment { get; set; }

        [StringLength(50)]
        public string GoingOut { get; set; }

        [StringLength(50)]
        public string FinancialStabiity { get; set; }

        [StringLength(50)]
        public string Religion { get; set; }

        [StringLength(50)]
        public string Fashion { get; set; }

        [StringLength(50)]
        public string Music { get; set; }
    }
}
