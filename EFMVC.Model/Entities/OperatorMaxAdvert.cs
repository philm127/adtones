using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model.Entities
{
    public class OperatorMaxAdvert
    {
        [Key]
        public int OperatorMaxAdvertId { get; set; }

        [StringLength(100)]
        public string KeyName { get; set; }

        [StringLength(10)]
        public string KeyValue { get; set; }

        public DateTime Addeddate { get; set; }

        public Nullable<DateTime> Updateddate { get; set; }

        public int OperatorId { get; set; }

        public Nullable<int> AdtoneServerOperatorMaxAdvertId { get; set; }
        public virtual Operator Operator { get; set; }
    }
}
