using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;

namespace EFMVC.Domain.Commands
{
   public class CreateOrUpdateQuestionCommentImagesCommand : ICommand
    {
        [Key]
        public int Id { get; set; }
        public int? QuestionCommentId { get; set; }

        public string UploadImages { get; set; }
    }
}
