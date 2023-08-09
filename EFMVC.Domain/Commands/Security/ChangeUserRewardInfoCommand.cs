using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;

namespace EFMVC.Domain.Commands.Security
{
   public class ChangeUserRewardInfoCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the user reward identifier.
        /// </summary>
        /// <value>The user reward identifier.</value>
        public int UserRewardId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the reward identifier.
        /// </summary>
        /// <value>
        /// The reward.
        /// </value>
        public int RewardId { get; set; }

        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        public int OperatorId { get; set; }
    }
}
