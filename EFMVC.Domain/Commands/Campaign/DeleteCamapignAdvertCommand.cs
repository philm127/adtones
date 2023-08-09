using EFMVC.CommandProcessor.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Commands.Campaign
{
    public class DeleteCamapignAdvertCommand : ICommand
    {
        public int CampaignAdvertId { get; set; }
    }
}
