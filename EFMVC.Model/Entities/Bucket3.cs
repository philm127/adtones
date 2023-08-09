using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFMVC.Model
{
    public class Bucket3
    {
        public int Id { get; set; }  
        public int? BUCKET_BATCH_ID { get; set; }
        //[ForeignKey("BUCKET_BATCH_ID")]
        //public  BucketBatch3 BucketBatch3 { get; set; }
       

        [StringLength(50)]
        public string MSISDN { get; set; }

        public int? ITEM_TOTAL { get; set; }

        [StringLength(255)]
        public string EMAIL_ADDRESS { get; set; }

        public int? MsUserProfileId { get; set; }
        public int? PromotionalUserId { get; set; }
    }
}
