using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model.Entities
{
    public class SoapUploadTone
    {
        public int Id { get; set; }
        public long UploadId { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
