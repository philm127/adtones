using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model
{
    public class CampaignmatchUsermatch7
    {
        public int Id { get; set; }

      
        public int? CampaignProfileId { get; set; }
        public virtual CampaignMatch7 CampaignMatch7  { get; set; }

        public int? UserProfileId { get; set; }
        public virtual UserMatch7 UserMatch7 { get; set; }

        [StringLength(256)]
        public string MEDIA_URL { get; set; }

        public float? BID_VALUE { get; set; }
        public DateTime? ADD_START { get; set; }
        public DateTime? ADD_END { get; set; }

        [StringLength(256)]
        public string DTMF_EVENT { get; set; }

        [StringLength(256)]
        public string SMS_MESSAGE { get; set; }

        [StringLength(256)]
        public string EMAIL_MESSAGE { get; set; }

        public int? ADD_STATE_ID { get; set; }

        [StringLength(50)]
        public string MSISDN { get; set; }

        [StringLength(256)]
        public string EMAIL_ADDRESS { get; set; }

        [StringLength(10)]
        public string CampaignTime { get; set; }

        [StringLength(10)]
        public string UserTime { get; set; }

        [StringLength(50)]
        public string MsUserProfileId { get; set; }

        [StringLength(50)]
        public string MSCampaignProfileId { get; set; }

    }
}
