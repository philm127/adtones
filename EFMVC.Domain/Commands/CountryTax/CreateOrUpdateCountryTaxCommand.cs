using EFMVC.CommandProcessor.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Commands
{
   public class CreateOrUpdateCountryTaxCommand : ICommand
    {
        public int Id { get; set; }
        public int? UserId { get; set; }

        public int? CountryId { get; set; }
        public decimal TaxPercantage { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public int Status { get; set; }

    }
}
