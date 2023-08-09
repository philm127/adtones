using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model
{
  public  class QuestionComment
    {
        [Key]
        public int Id { get; set; }

        public int? UserId { get; set; }
        public int? QuestionId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime ResponseDatetime { get; set; }
        public virtual User User { get; set; }

        public virtual Question Question { get; set; }

        public string TicketCode { get; set; }

        public virtual ICollection<QuestionCommentImages> QuestionCommentImages { get; set; }
    }
}
