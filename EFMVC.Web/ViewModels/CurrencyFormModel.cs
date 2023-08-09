using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.ViewModels
{
    public class CurrencyFormModel
    {
        public int CurrencyId { get; set; }
        public int CountryId { get; set; }
        public string CurrencyCode { get; set; }
    }
}