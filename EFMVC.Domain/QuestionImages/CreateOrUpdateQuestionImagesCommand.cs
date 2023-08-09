using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;

namespace EFMVC.Domain.Commands
{
   public class CreateOrUpdateQuestionImagesCommand : ICommand
    {
        [Key]
        public int Id { get; set; }

        public int? QuestionId { get; set; }
        public string UploadImage { get; set; }
    }
}
