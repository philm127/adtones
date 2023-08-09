using EFMVC.CommandProcessor.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Commands
{
    public class UpdateUserCreditCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int UserId { get; set; }

        public int CurrencyId { get; set; }

        /// <summary>
        /// Gets or sets the available credit.
        /// </summary>
        /// <value>
        /// The available credit.
        /// </value>
        public decimal AvailableCredit { get; set; }
    }
}
