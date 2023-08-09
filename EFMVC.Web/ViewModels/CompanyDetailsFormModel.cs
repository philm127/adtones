using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.ViewModels
{
    public class CompanyDetailsFormModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string Address { get; set; }
        //[Required]
        public string AdditionalAddress { get; set; }

        public string Town { get; set; }
        [Required]
        public string PostCode { get; set; }        
        [Required]
        public int CountryId { get; set; }
    }
}