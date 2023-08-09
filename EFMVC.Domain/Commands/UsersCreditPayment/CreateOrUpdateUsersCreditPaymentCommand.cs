using System;
using System.ComponentModel.DataAnnotations;
using EFMVC.CommandProcessor.Command;

namespace EFMVC.Domain.Commands
{
    public  class CreateOrUpdateUsersCreditPaymentCommand : ICommand
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int BillingId { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public int Status { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int? CampaignProfileId { get; set; }
    }
}
