using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model
{
    public class EmailVerificationCode
    {
        [Key]

        public int Id { get; set; }

        public int? UserId { get; set; }

        [StringLength(10)]
        public string VerificationCode { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual User User { get; set; }

    }
}
