using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Models
{
    public class BillingPaymentInfoDetailsWithMpesa
    {
        public int Id { get; set; }
        public int? BillingId { get; set; }
        public string InvoiceNumber { get; set; }
        public string PONumber { get; set; }

        public decimal Fundamount { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal TaxPercantage { get; set; }
        
        public string PhoneNumber { get; set; }
        
    }

    public class MPesaResponse
    {
        public string Status { get; set; }
        public string Description { get; set; }
        public string RequestId { get; set; }
    }
}