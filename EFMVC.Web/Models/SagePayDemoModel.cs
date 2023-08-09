using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Models
{
    public class SagePayDemoModel
    {
        [Required]
        public string CardholderName { get; set; }

        [Required]
        public string CardNumber { get; set; }

        public int Amount { get; set; }

        [Required]
        public string ExpiryDate { get; set; }

        [Required]
        public string SecurityCode { get; set; }
    }
}