using EFMVC.Model.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFMVC.Model
{
    public class PromotionalCampaign
    {
        [Key]
        public int ID { get; set; }
        public Nullable<int> OperatorID { get; set; }
        public string CampaignName { get; set; }
        public int BatchID { get; set; }
        public int MaxDaily { get; set; }
        public int MaxWeekly { get; set; }
        public string AdvertLocation { get; set; }
        public int Status { get; set; }
        public int? AdtoneServerPromotionalCampaignId { get; set; }

        public virtual Operator Operator { get; set; }
    }
}
