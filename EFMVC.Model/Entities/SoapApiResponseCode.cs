using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model.Entities
{
    public class SoapApiResponseCode
    {
        public int Id { get; set; }

        [StringLength(15)]
        public string ReturnCode { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
    }
}
