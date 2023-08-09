using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model.Entities
{
    public class MpesaHistory
    {
        [Key]
        public int MpesaHistoryId { get; set; }
        public int BillingId { get; set; }
        public string ReceiptNo { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }
        public string AccountReference { get; set; }
        public decimal Amount { get; set; }
        public string PhoneNumber { get; set; }

        public virtual Billing Billing { get; set; }
    }
}
