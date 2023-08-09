using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;

namespace EFMVC.Domain.Commands.CurrencyRate
{
    public class CreateOrUpdateCurrencyRateCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the currency rate identifier.
        /// </summary>
        /// <value>The currency rate identifier.</value>
        public int CurrencyRateId { get; set; }

        /// <summary>
        /// Gets or sets the Currency Code.
        /// </summary>
        /// <value>The Currency Code.</value>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the Currency Rate Amount.
        /// </summary>
        /// <value>
        /// The Currency Rate Amount.
        /// </value>
        public decimal CurrencyRateAmount { get; set; }

        /// <summary>
        /// Gets or sets the Updated Date.
        /// </summary>
        /// <value>
        /// The Updated Date.
        /// </value>
        public DateTime UpdatedDate { get; set; }
    }
}
