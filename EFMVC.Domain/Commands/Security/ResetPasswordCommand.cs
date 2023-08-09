using EFMVC.CommandProcessor.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Commands.Security
{
    public class ResetPasswordCommand : ICommand
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
    }
}
