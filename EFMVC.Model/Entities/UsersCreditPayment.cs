using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model
{
    public class UsersCreditPayment
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int BillingId { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public int Status { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public int? CampaignProfileId { get; set; }
        public virtual CampaignProfile CampaignProfile { get;set;}
        public virtual User User { get; set; }

        public virtual Billing Billing { get; set; }
    }
}
