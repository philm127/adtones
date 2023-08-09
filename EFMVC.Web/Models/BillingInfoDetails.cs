using System;


namespace EFMVC.Web.Models
{
    public class BillingInfoDetails
    {
        public decimal Maximumamountofcredit { get; set; }
        public decimal CreditAvailable { get; set; }
        public decimal CampaignFundsAvailable { get; set; }
        public int UserId { get; set; }
        public int? ClientId { get; set; }

        public int CampaignId { get; set; }

        public string InvoiceNumber { get; set; }
        public string PONumber { get; set; }

        //public decimal Fundamount { get; set; }

        //Add 15-02-2019
        public string Fundamount { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime PaymentDate { get; set; }

        public DateTime SettledDate { get; set; }

        public int Status { get; set; }

        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }

        public int CurrencyId { get; set; }

    }
}