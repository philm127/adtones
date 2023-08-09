using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.ViewModels
{
    public class AdvertCategoryFormModel
    {
        public int AdvertCategoryId { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(50, ErrorMessage = "Maximum Length Exceeded. Name cannot be more than 50 charaters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Country field is required.")]
        public int CountryId { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}