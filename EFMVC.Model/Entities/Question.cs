using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model
{
    public class Question
    {
        public Question()
        {
            QuestionImages = new HashSet<QuestionImages>();
            QuestionComment = new HashSet<QuestionComment>();
        }
        [Key]
        public int Id { get; set; }

        public int? UserId { get; set; }

        public string QNumber { get; set; }

        public int SubjectId { get; set; }

        public int? ClientId { get; set; }

        public int? CampaignProfileId { get; set; }

        public int? PaymentMethodId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public DateTime? LastResponseDateTime { get; set; }

        public DateTime? LastResponseDateTimeByUser { get; set; }
        
        public int Status { get; set; }

        public int? UpdatedBy { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public int? AdvertId { get; set; }
        public virtual Advert Advert { get; set; }

        public virtual User User { get; set; }      

        public virtual Client Client { get; set; }

        public virtual CampaignProfile CampaignProfile { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }

        public virtual ICollection<QuestionImages> QuestionImages { get; set; }

        public virtual ICollection<QuestionComment> QuestionComment { get; set; }

        public virtual QuestionSubject QuestionSubject { get; set; }
    }
}
