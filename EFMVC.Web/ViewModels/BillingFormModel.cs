using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EFMVC.Model;

namespace EFMVC.Web.ViewModels
{
    public class BillingFormModel
    {
        /// <summary>
     /// Gets or sets the advert identifier.
     /// </summary>
     /// <value>The advert identifier.</value>
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ClientId { get; set; }

        public int CampaignProfileId { get; set; }

        public int PaymentMethodId { get; set; }
        public string InvoiceNumber { get; set; }

        public decimal MaximumAmountCredit { get; set; }

        public decimal CreditAvailable { get; set; }


        public decimal CampaignFundsAvailable { get; set; }
        public string PONumber { get; set; }

        public decimal FundAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal TaxPercantage { get; set; }
        
        public DateTime PaymentDate { get; set; }

        public DateTime SettledDate { get; set; }

        public int Status { get; set; }
        public User User { get; set; }

        public Client Client { get; set; }

        public CampaignProfile CampaignProfile { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public ICollection<UsersCreditPayment> UsersCreditPayment { get; set; }

        public string CurrencyCode { get; set; }
    }
}