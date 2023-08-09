using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;

namespace EFMVC.Domain.Commands.Clients
{
    public class DeleteClientCommand : ICommand
    {
        /// <summary>
        /// The identifier
        /// </summary>
        public int Id;
    }
}
