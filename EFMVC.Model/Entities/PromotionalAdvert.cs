using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFMVC.Model
{
    public class PromotionalAdvert
    {
        [Key]
        public int ID { get; set; }
        public Nullable<int> CampaignID { get; set; }
        public string AdvertName { get; set; }
        public string AdvertLocation { get; set; }
        public int? AdtoneServerPromotionalAdvertId { get; set; }
        public virtual PromotionalCampaign PromotionalCampaign { get; set; }
    }
}
