using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.SearchClass
{
    public class CampaignAuditFilter
    {
        public string PlayID { get; set; }
        public string UserID { get; set; }

        public int CampaignProfileId { get; set; }
        public string FromPlayLength { get; set; }
        public string ToPlayLength { get; set; }

        public string Status { get; set; }

        public string FromPlayCost { get; set; }
        public string ToPlayCost { get; set; }

        public string FromTotalCost { get; set; }
        public string ToTotalCost { get; set; }
        public string SMSStatus { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime? StartTime { get; set; }
        ////[DataType(DataType.Date)]
        ////[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime? EndTime { get; set; }

        public string StartFromtime { get; set; }
        public string StartTotime { get; set; }

        public string EndFromtime { get; set; }
        public string EndTotime { get; set; }

    }
}