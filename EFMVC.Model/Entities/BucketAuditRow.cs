namespace EFMVC.Model
{
    public class BucketAuditRow
    {
        public int Id { get; set; }
        public int BucketAuditId { get; set; }
        public int State { get; set; }
        public string MediaUrl { get; set; }
        public string    BidValue { get; set; }
        public string Dtmf { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public int CampaignProfileId { get; set; }
        public int Sms { get; set; }
        public int Email { get; set; }
        public bool Processed { get; set; }

        public virtual BucketAudit BucketAudit { get; set; }
        public virtual CampaignProfile CampaignProfile { get; set; }
    }
}
