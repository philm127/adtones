using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;

namespace EFMVC.Domain.Commands
{
    public class CreateOrUpdateCampaignCreditPeriodCommand : ICommand
    {
        public int CampaignCreditPeriodId { get; set; }
        public int UserId { get; set; }
        public int CampaignProfileId { get; set; }
        public int CreditPeriod { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? AdtoneServerCampaignCreditPeriodId { get; set; }
    }
}
