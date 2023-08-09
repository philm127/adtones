using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model
{
    public class Country
    {
        [Key]

        public int Id { get; set; }

        public int? UserId { get; set; }

        public string Name { get; set; }
        public string ShortName { get; set; }

        [StringLength(50)]
        public string CountryCode { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public int Status { get; set; }

        public string TermAndConditionFileName { get; set; }

        public int? AdtoneServeCountryId { get; set; }

        public virtual User User { get; set; }

    }
}
