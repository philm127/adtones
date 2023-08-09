using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.OperatorAdmin.Models
{
    public class OperatorUserResult
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int NoOfactivecampaign { get; set; }
        public int NoOfunapprovedadverts { get; set; }
        public double Creditlimit { get; set; }
        public double Outstandinginvoice { get; set; }
        public int status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int TicketCount { get; set; }
    }
}