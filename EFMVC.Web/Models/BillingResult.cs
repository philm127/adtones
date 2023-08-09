using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Models
{
    public class BillingResult
    {
        public int ID { get; set; }
        public string InvoiceNO { get; set; }
        public string PONumber { get; set; }
        public int ClienId { get; set; }
        public string ClientName { get; set; }
        public int CampaignId { get; set; }
        public string CampaignName { get; set; }

        public DateTime InvoiceDate { get; set; }

        public decimal InvoiceTotal { get; set; }

        public String status { get; set; }
        public int fstatus { get; set; }

        public DateTime SettledDate { get; set; }

        public String MethodOfPayment { get; set; }

        public int PaymentMethodId { get; set; }

        public string CurrencySymbol { get; set; }

        public string CurrencyCode { get; set; }
    }
}