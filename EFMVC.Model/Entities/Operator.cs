using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace EFMVC.Model.Entities
{
    public class Operator
    {
        public int OperatorId { get; set; }

        [Required]
        [StringLength(50)]
        public string OperatorName { get; set; }

        public int? CountryId { get; set; }
        public int? AdtoneServerOperatorId { get; set; }
        public bool IsActive { get; set; }
        public virtual Country Country { get; set; }

        public decimal EmailCost { get; set; }
        public decimal SmsCost { get; set; }
        public int? CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }

    }
}
