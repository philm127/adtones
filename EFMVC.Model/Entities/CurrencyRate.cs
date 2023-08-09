using System;
using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model.Entities
{
    public class CurrencyRate
    {
        public int CurrencyRateId { get; set; }

        [StringLength(10)]
        public string CurrencyCode { get; set; }

        public decimal CurrencyRateAmount { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
