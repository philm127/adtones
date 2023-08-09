using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model
{
    public class AdvertRejection
    {       
        public int AdvertRejectionId { get; set; }
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        public int? AdvertId { get; set; }
        public virtual Advert Advert { get; set; }
        public string RejectionReason { get; set; }
        public DateTime CreatedDate { get; set; }
        public int?  AdtoneServerAdvertRejectionId { get; set; }

    }
}
