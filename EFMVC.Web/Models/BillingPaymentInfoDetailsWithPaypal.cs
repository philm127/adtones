﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Models
{
    public class BillingPaymentInfoDetailsWithPaypal
    {
        public int Id { get; set; }
        public int? BillingId { get; set; }
        public string InvoiceNumber { get; set; }
        public string PONumber { get; set; }

        public decimal Fundamount { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal TaxPercantage { get; set; }
        
        [Required]
        [EmailAddress]
        public string PaypalEmail { get; set; }

        public string PaypalTranID { get; set; }
    }
}