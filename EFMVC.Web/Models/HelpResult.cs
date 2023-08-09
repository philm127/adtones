using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Models
{
    public class HelpResult
    {
        public int Id { get; set; }

        public string QANumber { get; set; }

        public int? ClientId { get; set; }

        public string ClientName { get; set; }

        public int? CampaignProfileId { get; set; }

        public string CampaignName { get; set; }

        //[DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}")]
        //public DateTime? QuestionDateTime { get; set; }
        public DateTime? QuestionDateTimeSort { get; set; }
        public string QuestionDateTime { get; set; }

        public string QuestionTitle { get; set; }

        public int? QuestionSubjectId { get; set; }
        public string QuestionSubject { get; set; }

        public int Status { get; set; }
        public string fStatus { get; set; }
        //public DateTime? LastResponseDatetime { get; set; }
        public DateTime? LastResponseDatetimeSort { get; set; }
        public string LastResponseDatetime { get; set; }

        public int? PaymentMethodId { get; set; }
    }
}