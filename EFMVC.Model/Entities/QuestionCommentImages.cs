using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model
{
   public class QuestionCommentImages
    {
        [Key]
        public int Id { get; set; }
        public int? QuestionCommentId { get; set; }

        public string UploadImages { get; set; }
    }
}
