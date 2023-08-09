using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.ViewModels
{
    public class CountryTaxFormModel
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        [Required]
        public int? CountryId { get; set; }
        //[Required]
        //[Range(0, 100, ErrorMessage = "Percantage must be between 0 to 100")]
        //[RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid percantage")]
        [Required(ErrorMessage = "The TaxPercentage field is required.")]
        [Range(0, 100, ErrorMessage = "Percentage must be between 0 to 100")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid percentage")]
        public decimal TaxPercantage { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public int Status { get; set; }
    }
}