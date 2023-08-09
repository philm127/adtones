using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model.Entities
{
   public class LinuxServerConnectionString
    {
        [Key]
        public int Id { get; set; }
        public int? OperatorId { get; set; }
        [StringLength(1000)]
        public string ConnectionString { get; set; }
    }
}
