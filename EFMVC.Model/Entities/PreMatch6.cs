using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model
{
    public class PreMatch6
    {
        public int PreMatch6Id { get; set; }
              
        public int? CampaignMatchId { get; set; }
        
        [StringLength(256)]
        public string MEDIA_URL { get; set; }

        public float? BID_VALUE { get; set; }
       
        [StringLength(256)]
        public string SMS_MESSAGE { get; set; }

        [StringLength(256)]
        public string EMAIL_MESSAGE { get; set; }

        public int? ADD_STATE_ID { get; set; }

        [StringLength(50)]
        public string MSISDN { get; set; }

        [StringLength(256)]
        public string EMAIL_ADDRESS { get; set; }
        
        [StringLength(50)]
        public string MsUserProfileId { get; set; }

        [StringLength(50)]
        public string MSCampaignProfileId { get; set; }

    }
}
