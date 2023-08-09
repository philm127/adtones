using EFMVC.CommandProcessor.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Commands.Campaign
{
  public class ChangeCampaignStatusCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the campaign profile identifier.
        /// </summary>
        /// <value>
        /// The campaign profile identifier.
        /// </value>
        public int CampaignProfileId { get; set; }


        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public int Status { get; set; }
    }
}
