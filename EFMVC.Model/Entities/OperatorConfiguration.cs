using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace EFMVC.Model.Entities
{
    public class OperatorConfiguration
    {
        [Key]
        public int OperatorConfigurationId { get; set; }
        public int OperatorId { get; set; }
        public int Days { get; set; }
        public int? AdtoneServerOperatorConfigurationId { get; set; }
        public bool IsActive { get; set; }
        public DateTime AddedDate { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public virtual Operator Operator { get; set; }
    }
}