using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.EFWebPDF
{
    public class Customer
    {
        public string FullName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public int CustomerID { get; private set; }

        //Constructors
        public Customer()
        {
            CustomerID = 321;
        }
    }
}