using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Models
{
    public class HelpAdminResult
    {
        public int Id { get; set; }

        public int ? userId { get; set; }
        public string userName { get; set; }

        public string userEmail { get; set; }
        public string QANumber { get; set; }

        public int? ClientId { get; set; }

        public string ClientName { get; set; }

        public int? CampaignProfileId { get; set; }

        public string CampaignName { get; set; }

        public DateTime? QuestionDateTime { get; set; }

        public string QuestionTitle { get; set; }

        public int? QuestionSubjectId { get; set; }
        public string QuestionSubject { get; set; }

        public int Status { get; set; }
        public string fStatus { get; set; }
        public DateTime? LastResponseDatetime { get; set; }

        public DateTime? LastResponseDateTimeByUser { get; set; }

        public int? PaymentMethodId { get; set; }
    }
}