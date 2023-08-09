using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.EFWebPDF
{
    public class Invoice
    {
        public Customer Customer { get; set; }
        public int Items { get; set; }
        public Item Item1 { get; set; }
        public List<Item> ItemList { get; set; }

        //private set
        public DateTime Date { get; private set; }

        public DateTime? SettledDate { get;  set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceCountry { get; set; }

        public string InvoiceTax { get; set; }
        public string MethodOfPayment { get; set; }
        public int? typeOfPayment { get; set; }
        public string PONumber { get; set; }
        public string Imagepath { get; set; }
        public int MyProperty { get; set; }
        public decimal vat { get; set; }
       
        public int? CountryId { get; set; }
        public string CurrencySymbol { get; set; }

        //Constructors
        public Invoice()
        {
            //In reality would get from database for unique number
            Date = DateTime.Today;

        }

        public Invoice(Item Item1)
            : this()
        {
            ItemList = new List<Item>();
            ItemList.Add(Item1);
        }

        public Invoice(Item Item1, Item Item2)
            : this()
        {
            ItemList = new List<Item>();
            ItemList.Add(Item1);
            ItemList.Add(Item2);
        }

        public Invoice(Item Item1, Item Item2, Item Item3)
            : this()
        {
            ItemList = new List<Item>();
            ItemList.Add(Item1);
            ItemList.Add(Item2);
            ItemList.Add(Item3);
        }

        public Invoice(Item Item1, Item Item2, Item Item3, Item Item4)
            : this()
        {
            ItemList = new List<Item>();
            ItemList.Add(Item1);
            ItemList.Add(Item2);
            ItemList.Add(Item3);
            ItemList.Add(Item4);
        }

        public Invoice(Item Item1, Item Item2, Item Item3, Item Item4, Item Item5)
            : this()
        {
            ItemList = new List<Item>();
            ItemList.Add(Item1);
            ItemList.Add(Item2);
            ItemList.Add(Item3);
            ItemList.Add(Item4);
            ItemList.Add(Item5);
        }

        public decimal GetSubtotal()
        {



            decimal total = 0;

            foreach (var item in ItemList)
            {
                total += item.ItemTotal();
            }
            return total;

        }

        public decimal GetVATAmount()
        {
            decimal total = GetSubtotal();
            total *= vat;

            return total;
        }

        public decimal GetTotal()
        {
            decimal total = GetSubtotal() + GetVATAmount();
            return total;
        }

    }
}