using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.SearchClass
{
    public class UserFilter
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public DateTime? Fromdate { get; set; }
        //public DateTime? Todate { get; set; }
        public string Fromdate { get; set; }
        public string Todate { get; set; }
        public int Status { get; set; }
       
    }
}