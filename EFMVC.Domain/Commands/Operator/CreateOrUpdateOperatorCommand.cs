using EFMVC.CommandProcessor.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Commands
{
   public class CreateOrUpdateOperatorCommand : ICommand
    {
        public int OperatorId { get; set; }
       
        public string OperatorName { get; set; }

        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public bool IsActive { get; set; }
        public decimal EmailCost { get; set; }
        public decimal SmsCost { get; set; }
        public int CurrencyId { get; set; }
    }
}
