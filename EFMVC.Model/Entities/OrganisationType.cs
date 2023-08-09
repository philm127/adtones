using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace EFMVC.Model.Entities
{
    public class OrganisationType
    {
        public int OrganisationTypeId { get; set; }

        [StringLength(50)]
        public string Type { get; set; }
     
    }
}
