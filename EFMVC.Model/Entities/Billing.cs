using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model
{
   public class Billing
    {
        [Key]
        public int Id { get; set; }

        public int? UserId { get; set; }
        public int? ClientId { get; set; }

        public int? CampaignProfileId { get; set; }

        public int? PaymentMethodId { get; set; }

        public string InvoiceNumber { get; set; }

        public string PONumber { get; set; }

        public decimal FundAmount { get; set; }

        public decimal TaxPercantage { get; set; }
        
        public decimal TotalAmount { get; set; }

        public DateTime PaymentDate { get; set; }
        public DateTime SettledDate { get; set; }

        public int Status { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorDescription { get; set; }

        public int? AdtoneServerBillingId { get; set; }

        public virtual User User { get; set; }

        public virtual Client Client { get; set; }

        public virtual CampaignProfile CampaignProfile { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }

        public virtual ICollection<UsersCreditPayment> UsersCreditPayment { get; set; }

        public string CurrencyCode { get; set; }

    }
}
