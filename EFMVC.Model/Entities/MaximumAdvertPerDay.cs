using EFMVC.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model
{
    public class MaximumAdvertPerDay
    {
        [Key]
        public int Id { get; set; }
        public int? UserProfileId { get; set; }
        public int RemainingAdvert { get; set; }
        public DateTime AddedDate { get; set; }
        public int? PromotionalUserId { get; set; }
    }
}
