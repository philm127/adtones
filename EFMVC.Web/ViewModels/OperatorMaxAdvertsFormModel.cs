using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.ViewModels
{
    public class OperatorMaxAdvertsFormModel
    {

        public int OperatorMaxAdvertId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "KeyName Field Maximum length is 100")]
        public string KeyName { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "KeyValue Field Maximum length is 10")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "KeyValue Must be Numeric")]
        public string KeyValue { get; set; }

        public DateTime Addeddate { get; set; }

        public Nullable<DateTime> Updateddate { get; set; }

        [Required]
        public int OperatorId { get; set; }
        public string OperatorName { get; set; }
    }
}