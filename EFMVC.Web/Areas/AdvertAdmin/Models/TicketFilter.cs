using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.AdvertAdmin.Models
{
    public class TicketFilter
    {
        public string ID { get; set; }
        public int UserId { get; set; }
        public int ClientId { get; set; }

        //public DateTime? Fromdate { get; set; }
        //public DateTime? Todate { get; set; }

        //public DateTime? LastResponseFromdate { get; set; }
        //public DateTime? LastResponseTodate { get; set; }

        public string Fromdate { get; set; }
        public string Todate { get; set; }

        public string LastResponseFromdate { get; set; }
        public string LastResponseTodate { get; set; }

        public int SubjectId { get; set; }

        public int Status { get; set; }

        public int PaymentMethodId { get; set; }
    }
}