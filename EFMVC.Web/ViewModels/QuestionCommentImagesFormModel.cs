using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.ViewModels
{
    public class QuestionCommentImagesFormModel
    {
        [Key]
        public int Id { get; set; }
        public int? QuestionCommentId { get; set; }

        public string UploadImages { get; set; }
    }
}