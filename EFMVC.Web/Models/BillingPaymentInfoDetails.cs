using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Models
{
    public class BillingPaymentInfoDetails
    {
        public int Id { get; set; }
        public int? BillingId { get; set; }
        public string InvoiceNumber { get; set; }
        public string PONumber { get; set; }
        public decimal TotalFundAmount { get; set; }
       
        public decimal Fundamount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxPercantage { get; set; }
        [Required]
        public string CardType { get; set; }

        [Required]
        public string CardNumber { get; set; }
        [Required]
        public string ExpiryMonth { get; set; }
        [Required]
        public string ExpiryYear { get; set; }
        [Required]
        public string NameOfCard { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string SecurityCode { get; set; }
        [Required]
        public string BillingAddress { get; set; }        
        public string BillingAddress2 { get; set; }
        [Required]
        public string BillingTown { get; set; }

        [Required]
        public string BillingPostcode { get; set; }

        public string PaypalTranID { get; set; }
    }
}