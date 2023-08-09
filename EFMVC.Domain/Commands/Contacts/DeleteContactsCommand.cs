using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;

namespace EFMVC.Domain.Commands.Contacts
{
    public class DeleteContactsCommand : ICommand
    {
        public int Id;
    }
}
