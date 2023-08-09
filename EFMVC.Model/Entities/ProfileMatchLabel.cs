using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model.Entities
{
    public class ProfileMatchLabel
    {
        [Key]
        public int Id { get; set; }

        public int ProfileMatchInformationId { get; set; }

        [StringLength(100)]
        public string ProfileLabel { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual ProfileMatchInformation ProfileMatchInformation { get; set; }
    }
}
