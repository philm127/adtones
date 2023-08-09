using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.Models
{
    public class CopyRightResult
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedDateSort { get; set; }
        public string CreatedDate { get; set; }
    }
}