using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Collections.Generic;


namespace EFMVC.Model {
    
    public class CampaignProfileRadio {
        [Key]
        public int CampaignProfileRadioId { get; set; }
        public int CampaignProfileId { get; set; }
        public CampaignProfile CampaignProfile { get; set; }

        [StringLength(50)]
        public string National { get; set; }

        [StringLength(50)]
        public string Local { get; set; }

        [StringLength(50)]
        public string Music { get; set; }

        [StringLength(50)]
        public string Sport { get; set; }

        [StringLength(50)]
        public string Talk { get; set; }
    }
}
