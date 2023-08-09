using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model
{
  public class QuestionImages
    {
        [Key]
        public int Id { get; set; }

        public int? QuestionId { get; set; }
        public string UploadImage { get; set; }
    }
}
