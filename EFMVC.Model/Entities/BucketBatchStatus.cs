using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model
{
    public class BucketBatchStatus
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string STATUS { get; set; }

    }
}
