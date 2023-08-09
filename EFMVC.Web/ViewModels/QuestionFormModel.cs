using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFMVC.Model;

namespace EFMVC.Web.ViewModels
{
    public class QuestionFormModel
    {
        [Key]
        public int Id { get; set; }

        public int? UserId { get; set; }

        public string QNumber { get; set; }

        [Required]
        public int SubjectId { get; set; }

       
        public int? ClientId { get; set; }

      
        public int? CampaignProfileId { get; set; }
        
        public int? PaymentMethodId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [AllowHtml]
        [StringLength(200, ErrorMessage = "The Description field cannot be more than 200 characters.")]
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? LastResponseDateTime { get; set; }

        public DateTime? LastResponseDateTimeByUser { get; set; }
        
        public int Status { get; set; }

        public int? UpdatedBy { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public int? AdvertId { get; set; }
        public QuestionSubject QuestionSubject { get; set; }
        public User User { get; set; }

        public Client Client { get; set; }

        public CampaignProfile CampaignProfile { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public ICollection<QuestionImages> QuestionImages { get; set; }

        public ICollection<QuestionComment> QuestionComment { get; set; }
    }
}