using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.Models
{
    public class RewardResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DateTime? CreatedDateSort { get; set; }
        public string CreatedDate { get; set; }

        //Add 21-02-2019
        public int OperatorId { get; set; }
        public string OperatorName { get; set; }
    }
}