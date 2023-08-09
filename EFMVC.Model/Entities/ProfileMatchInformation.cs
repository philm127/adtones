using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model.Entities
{
    public class ProfileMatchInformation
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string  ProfileName { get; set; }

        public bool IsActive { get; set; }

        public int? CountryId { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string ProfileType { get; set; }
        public virtual Country Country { get; set; }
    }
}
