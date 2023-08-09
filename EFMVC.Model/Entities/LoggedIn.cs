using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model.Entities
{
    public class LoggedIn
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string SessionId { get; set; }

        public bool IsLoggedIn { get; set; }
    }
}
