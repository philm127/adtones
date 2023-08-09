using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Collections.Generic;


namespace EFMVC.Model {
    
    public class CampaignProfileTv {
        [Key]
        public int CampaignProfileTvId { get; set; }
        public int CampaignProfileId { get; set; }
        public CampaignProfile CampaignProfile { get; set; }

        [StringLength(50)]
        public string Satallite { get; set; }

        [StringLength(50)]
        public string Cable { get; set; }

        [StringLength(50)]
        public string Terrestrial { get; set; }

        [StringLength(50)]
        public string Internet { get; set; }
    }
}
