using System.Collections.Generic;

namespace EFMVC.Model
{
    public class BucketAudit
    {
        public BucketAudit()
        {
            BucketAuditRows = new HashSet<BucketAuditRow>();
        }

        public int Id { get; set; }
        public int BucketId { get; set; }
        public string MSISDN { get; set; }
        public string BucketPeriodStart { get; set; }
        public string TargetDeliveryServer { get; set; }
        public bool Processed { get; set; }

        public virtual ICollection<BucketAuditRow> BucketAuditRows { get; set; }
    }
}
