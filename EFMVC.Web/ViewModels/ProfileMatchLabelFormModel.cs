using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.ViewModels
{
    public class ProfileMatchLabelFormModel
    {
        public int Id { get; set; }

        public int ProfileMatchInformationId { get; set; }

        [StringLength(100)]
        [Required]
        public string ProfileLabel { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}