using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.ViewModel
{
    public class PromotionalUserFormModel
    {
        public int ID { get; set; }

        [Required]
        public int CountryId { get; set; }

        [Required]
        public int OperatorId { get; set; }

        [Required]
        public int BatchID { get; set; }

        public List<string> Files { get; set; }
    }
}