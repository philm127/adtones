using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model
{
   public class UsersCredit
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public decimal AssignCredit { get; set; }

        public decimal AvailableCredit { get; set; }

        public int CurrencyId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
      
        public virtual User User { get; set; }

    }
}
