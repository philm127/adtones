using EFMVC.CommandProcessor.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Commands
{
    public class ChangePromotionalCampaignStatusCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the promotional campaign identifier.
        /// </summary>
        /// <value>
        /// The promotional campaign identifier.
        /// </value>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public int Status { get; set; }
    }
}
