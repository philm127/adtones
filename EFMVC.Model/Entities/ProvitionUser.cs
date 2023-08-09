
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFMVC.Model
{
    public class ProvitionUser
    {
        [Key]
        public int ProvitionUserID { get; set; }
        public int? PromotionalUserId { get; set; }
        public int? UserProfileId { get; set; }
        public string MSISDN { get; set; }
        public int? DTMFKey { get; set; }
    }
}
