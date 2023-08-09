using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;

namespace EFMVC.Domain.Commands
{
   public class CreateOrUpdateQuestionCommentCommand : ICommand
    {
        [Key]
        public int Id { get; set; }

        public int? UserId { get; set; }
        public int? QuestionId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string TicketCode { get; set; }

        public DateTime ResponseDatetime { get; set; }
    }
}
