using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.ViewModels
{
    public class RewardFormModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Value field is required.")]
        public string Value { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        //Add 21-02-2019
        [Required(ErrorMessage = "The Operator field is required.")]
        public int OperatorId { get; set; }
    }
}