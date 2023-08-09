using EFMVC.CommandProcessor.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Commands.Security
{
    public class UserPasswordHistoryCommand : ICommand
    {
        public int UserPasswordHistoryId { get; set; }

        public int UserId { get; set; }

        public string PasswordHash { get; set; }

        public DateTime DateCreated { get; set; }

        public int? AdtoneServerUserPasswordHistoryId { get; set; }
    }
}
