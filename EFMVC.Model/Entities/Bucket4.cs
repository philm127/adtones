using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFMVC.Model
{
    public class Bucket4
    {
        public int Id { get; set; }  
        public int? BUCKET_BATCH_ID { get; set; }
        //[ForeignKey("BUCKET_BATCH_ID")]
        //public  BucketBatch4 BucketBatch4 { get; set; }
        

        [StringLength(50)]
        public string MSISDN { get; set; }

        public int? ITEM_TOTAL { get; set; }

        [StringLength(255)]
        public string EMAIL_ADDRESS { get; set; }

        public int? MsUserProfileId { get; set; }
        public int? PromotionalUserId { get; set; }
    }
}
