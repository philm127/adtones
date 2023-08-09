using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.EFWebPDF
{
    public class Item
    {
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Organisation { get; set; }

        public decimal ItemTotal()
        {
            decimal total = Price * Quantity;
            return total;
        }

    }
}