using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace EFMVC.Web.Areas.Users.Models
{
    public class BlockedNumberResult
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TelephoneNumber { get; set; }
        public string Name { get; set; }

        public bool Active { get; set; }
    }
}