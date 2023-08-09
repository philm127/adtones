using System;

namespace EFMVC.Model
{
    public class UserProfileAdvertsReceived
    {
        public int Id { get; set; }
        public int UserProfileId { get; set; }
        public string AdvertRef { get; set; }
        public string AdvertName { get; set; }
        public string Brand { get; set; }
        public string FileName { get; set; }
        public DateTime DateTimePlayed { get; set; }
        public string CreditsReceived { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public int? CampaignAuditId { get; set; }
        public int Proceed { get; set; }
        public DateTime? AddedDate { get; set; }
        public bool IsMaxAdvertPerDay { get; set; }
        public virtual CampaignAudit CampaignAudit { get; set; }

        public bool IsRewardReceived { get; set; }
        public string UnUsedCredit { get; set; }
    }
}
