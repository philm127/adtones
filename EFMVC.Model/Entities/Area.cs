using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model.Entities
{
    public class Area
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public bool IsActive { get; set; }
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
    }
}
