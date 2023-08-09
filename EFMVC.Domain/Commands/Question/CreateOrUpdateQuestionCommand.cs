using System;
using System.ComponentModel.DataAnnotations;
using EFMVC.CommandProcessor.Command;

namespace EFMVC.Domain.Commands
{
    public class CreateOrUpdateQuestionCommand : ICommand
    {
        [Key]
        public int Id { get; set; }

        public int? UserId { get; set; }

        public string QNumber { get; set; }

        public int SubjectId { get; set; }

        public int? ClientId { get; set; }

        public int? CampaignProfileId { get; set; }

        public int? PaymentMethodId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public int? AdvertId { get; set; }

        public int? UpdatedBy { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public DateTime? LastResponseDateTime { get; set; }

        public DateTime? LastResponseDateTimeByUser { get; set; }
        
        public int Status { get; set; }
    }

}
