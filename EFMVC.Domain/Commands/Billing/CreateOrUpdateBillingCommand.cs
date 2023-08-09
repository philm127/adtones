using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;

namespace EFMVC.Domain.Commands.Billing
{
    public class CreateOrUpdateBillingCommand : ICommand
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

        public string CurrencyCode { get; set; }
    }
}
