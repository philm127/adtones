using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Models
{
    public class ClientResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NoOfCompaign { get; set; }

        public DateTime? CreatedDateSort { get; set; }
        public string CreatedDate { get; set; }

        public decimal TotalBudget { get; set; }

        public decimal TotalSpend { get; set; }

        public decimal TotalPlays { get; set; }

        public decimal AvgBid { get; set; }

        public string Status { get; set; }

        public int fStatus { get; set; }

        public string CurrencySymbol { get; set; }

        public string CurrencyCode { get; set; }

        public int? userCurrencyId { get; set; }

    }
}