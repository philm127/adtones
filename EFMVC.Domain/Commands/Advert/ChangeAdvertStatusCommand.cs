using EFMVC.CommandProcessor.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Commands
{
    public class ChangeAdvertStatusCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the advert identifier.
        /// </summary>
        /// <value>
        /// The advert identifier.
        /// </value>
        public int AdvertId { get; set; }

        public int? UpdatedBy { get; set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public int Status { get; set; }
    }
}
