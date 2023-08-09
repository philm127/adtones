using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;

namespace EFMVC.Domain.Commands
{
   public class DeleteCampaignCreditPeriodCommand : ICommand
    {
        public int Id;
    }
}
