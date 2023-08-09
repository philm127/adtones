using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace EFMVC.Model.Entities
{
    public class AdvertCategory
    {
        public int AdvertCategoryId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public int? CountryId { get; set; }

        public int? AdtoneServerAdvertCategoryId { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Country Country { get; set; }
     
    }
}
