using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.ViewModels
{
    public class OperatorFormModel
    {
        public int OperatorId { get; set; }

        [Required(ErrorMessage = "The Operator Name field is required.")]
        [StringLength(50)]
        public string OperatorName { get; set; }

        [Required(ErrorMessage = "The Country Name field is required.")]
        public int CountryId { get; set; }
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "The Email Cost field is required.")]
        public decimal EmailCost { get; set; }

        [Required(ErrorMessage = "The SMS Cost field is required.")]
        public decimal SmsCost { get; set; }

        [Required(ErrorMessage = "The Currency field is required.")]
        public int CurrencyId { get; set; }
        // public string  CountryName { get; set; }

    }
}