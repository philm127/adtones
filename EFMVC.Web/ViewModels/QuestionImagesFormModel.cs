using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.ViewModels
{
    public class QuestionImagesFormModel
    {
        [Key]
        public int Id { get; set; }

        public int? QuestionId { get; set; }
        public string UploadImage { get; set; }
    }
}