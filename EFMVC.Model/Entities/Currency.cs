using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace EFMVC.Model.Entities
{
    public class Currency
    {
        public int CurrencyId { get; set; }

        [StringLength(10)]
        public string CurrencyCode { get; set; }

        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }
    }
}
