using EFMVC.CommandProcessor.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Commands
{
   public class CreateOrUpdatePromotionalCampaignCommand : ICommand
    {
        public int ID { get; set; }
        public Nullable<int> OperatorID { get; set; }
        public string CampaignName { get; set; }
        public int BatchID { get; set; }
        public int MaxDaily { get; set; }
        public int MaxWeekly { get; set; }
        public string AdvertLocation { get; set; }
        public int Status { get; set; }
        public int? AdtoneServerPromotionalCampaignId { get; set; }
    }
}
