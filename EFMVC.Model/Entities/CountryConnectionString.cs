using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model.Entities
{
   public class CountryConnectionString
    {
        [Key]
        public int Id { get; set; }
        public int? CountryId { get; set; }
        public int? OperatorId { get; set; }
        public string ConnectionString { get; set; }
    }
}
