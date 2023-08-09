using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;

namespace EFMVC.Domain.Commands.CompanyDetails
{
   public class DeleteCampanyDetailsCommand : ICommand
    {
        public int Id;
    }
}
