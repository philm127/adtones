using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model.Entities
{
    public class UserPasswordHistory
    {
        public int UserPasswordHistoryId { get; set; }

        public int UserId { get; set; }

        public string PasswordHash { get; set; }

        public DateTime DateCreated { get; set; }

        public int? AdtoneServerUserPasswordHistoryId { get; set; }

        public virtual User User { get; set; }
    }
}
