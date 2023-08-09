using EFMVC.CommandProcessor.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Commands
{
    public class DeleteAreaCommand : ICommand
    {
        public int AreaId;
    }
}
