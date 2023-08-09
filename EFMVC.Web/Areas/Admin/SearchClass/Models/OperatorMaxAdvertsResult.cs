using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.Models
{
    public class OperatorMaxAdvertsResult
    {
        public int OperatorMaxAdvertId { get; set; }
        public string KeyName { get; set; }
        public string KeyValue { get; set; }
        public string Addeddate { get; set; }

        public int OperatorId { get; set; }

        public string OperatorName { get; set; }
    }
}