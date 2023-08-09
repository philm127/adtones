using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Models
{
    public class SagePayBillingDetails
    {

        public int Id { get; set; }
        public int? BillingId { get; set; }
        public string InvoiceNumber { get; set; }
        public string PONumber { get; set; }

        public decimal Fundamount { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal TaxPercantage { get; set; }
        [Required]
        public string CardType { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public string ExpiryMonth { get; set; }
        [Required]
        public string ExpiryYear { get; set; }
        [Required]
        public string NameOfCard { get; set; }
        [Required]
        public string SecurityCode { get; set; }
        [Required]
        public string BillingAddress { get; set; }
        public string BillingAddress2 { get; set; }
        [Required]
        public string BillingTown { get; set; }

        [Required]
        public string BillingPostcode { get; set; }

        public string SagePayTranID { get; set; }    
    }

    public class Card
    {
        public string Expiry { get; set; }
        public string MerchantSessionKey { get; set; }
        public string StatusCode { get; set; }
        public List<Errors> errors { get; set; }
    }

    public class CardIdentifiers
    {
        public string CardIdentifier { get; set; }
        public DateTime Expiry { get; set; }
        public string CardType { get; set; }
        public List<Errors> errors { get; set; }
        public string StatusDetail { get; set; }
    }

    public class Output
    {
        public string Status { get; set; }
        public string StatusDetail { get; set; }
        public string TransactionId { get; set; }
        public string TransactionType { get; set; }
        public string Currency { get; set; }
        public List<Errors> errors { get; set; }
    }

    public class Errors
    {
        public string description { get; set; }
        public string property { get; set; }
        public string clientMessage { get; set; }
        public string code { get; set; }
    }

    //public class Errors
    //{
    //    public string description { get; set; }
    //    public string property { get; set; }
    //    public string clientMessage { get; set; }
    //    public string code { get; set; }
    //}
}