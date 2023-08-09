using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EFMVC.Model;

namespace EFMVC.Web.ViewModels
{
    public class QuestionCommentFormModel
    {
        [Key]
        public int Id { get; set; }

        public int? UserId { get; set; }
        public int? QuestionId { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "The Description field cannot be more than 200 characters.")]
        public string Description { get; set; }

        public DateTime ResponseDatetime { get; set; }
        public User User { get; set; }

        public Question Question { get; set; }
        public string TicketCode { get; set; }

        public ICollection<QuestionCommentImages> QuestionCommentImages { get; set; }
    }
}