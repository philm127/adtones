using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;

namespace EFMVC.Domain.Commands.Billing
{
   public class DeleteBillingCommand : ICommand
    {
        public int Id;
        public int CampaignId;
        public int ClientId;
    }
}
