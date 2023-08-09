using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;

namespace EFMVC.Domain.Commands.BillingDetails
{
  public  class DeleteBillingDetailsCommand : ICommand
    {
        public int Id;
    }
}
