﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.Models
{
    public class UserAdvertResult
    {
    
        public int AdvertId { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string MediaFileLocation { get; set; }
        public int? ClientId { get; set; }
        public string ClientName { get; set; }
        public string Scripts { get; set; }

        public string ScriptsPath { get; set; }
        public int userId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public DateTime? CreatedDateSort { get; set; }
        public string CreatedDate { get; set; }

        public int status { get; set; }
        public string fstatus { get; set; }

    }
}