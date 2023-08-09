using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.ViewModels
{
    public class CountryFormModel
    {
        public int Id { get; set; }
        public int? UserId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string ShortName { get; set; }
        [Required(ErrorMessage ="The Code field is required.")]
        public string CountryCode { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public int Status { get; set; }

       // [Required(ErrorMessage = "The Term & Condition field is required.")]
        public string TermAndConditionFileName { get; set; }
    }
}