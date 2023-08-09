using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.AdvertAdmin.Models
{
    public class TicketResult
    {
        public int Id { get; set; }

        public int userId { get; set; }

        public int? fuserId { get; set; }
        public string userName { get; set; }

        public string userEmail { get; set; }

        public string Organisation { get; set; }
        public string QANumber { get; set; }

        public int? ClientId { get; set; }
        public int? fClientId { get; set; }

        public string ClientName { get; set; }

        public int? CampaignProfileId { get; set; }

        public string CampaignName { get; set; }

        public DateTime? QuestionDateTimeSort { get; set; }
        public string QuestionDateTime { get; set; }

        public string QuestionTitle { get; set; }

        public int QuestionSubjectId { get; set; }
        public int? fQuestionSubjectId { get; set; }
        public string QuestionSubject { get; set; }

        public int Status { get; set; }
        public string fStatus { get; set; }
        public DateTime? LastResponseDatetimeSort { get; set; }
        public string LastResponseDatetime { get; set; }

        public DateTime? LastResponseDateTimeByUserSort { get; set; }
        public string LastResponseDateTimeByUser { get; set; }

        public int? PaymentMethodId { get; set; }

        public int? fPaymentMethodId { get; set; }
    }
}