using System;
using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model
{
    public class PromotionalCampaignAudit
    {
        [Key]
        public int PromotionalCampaignAuditId { get; set; }
        public int PromotionalCampaignId { get; set; }
        public string MSISDN { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Int64 PlayLengthTicks { get; set; }
        public string DTMFKey { get; set; }
        public DateTime? AddedDate { get; set; }
        public int? AdtoneServerPromotionalCampaignId { get; set; }
        public bool AdtoneServerDataTransfer { get; set; }

        public PromotionalCampaign PromotionalCampaign { get; set; }
    }
}
