using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model.Entities
{
   public class GravityFormsTrack
    {
        [Key]
        public int Id { get; set; }
        public int? GravityFormsId { get; set; }
        public string Email { get; set; }
        public string MSISDN { get; set; }
    }
}
