using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFMVC.Model
{
    public class BucketItem7
    {
        public int Id { get; set; }  
        public int? BUCKET_ID { get; set; }
        [ForeignKey("BUCKET_ID")]
        public Bucket7 Bucket7 { get; set; }

        public int? PRIORITY { get; set; }
        public int? ADD_STATE_ID { get; set; }

        [StringLength(256)]
        public string MEDIA_URL { get; set; }
        public float? BID_VALUE { get; set; }
        public DateTime? ADD_START { get; set; }
        public DateTime? ADD_END { get; set; }

        [StringLength(256)]
        public string CAMPAIGNID { get; set; }

        [StringLength(256)]
        public string DTMF_EVENT { get; set; }

        [StringLength(256)]
        public string SMS_MESSAGE { get; set; }

        [StringLength(256)]
        public string EMAIL_MESSAGE { get; set; }

        [StringLength(256)]
        public string ORIGINATOR { get; set; }


        public bool? Processed { get; set; }

        public int? PlayLengthTicks { get; set; }
        public float? SMSCost { get; set; }
        public float? EmailCost { get; set; }
        public float? TotalCost { get; set; }

        [StringLength(256)]
        public string MSCampaignProfileId { get; set; }

        public int? PromotionalCampaignId { get; set; }
    }
}
