using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.SearchClass
{
    public class ClientFilter
    {
        public string ClientId { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}", ApplyFormatInEditMode = true)]
        public string Fromdate { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}", ApplyFormatInEditMode = true)]
        public string Todate { get; set; }

        public string FromBudget { get; set; }
        public string ToBudget { get; set; }

        public string FromSpend { get; set; }
        public string ToSpend { get; set; }

        public string Frombid { get; set; }
        public string Tobid { get; set; }

        public String Status { get; set; }
    }
}