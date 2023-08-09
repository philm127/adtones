using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Collections.Generic;


namespace EFMVC.Model {
    
    public class UserProfileTv {
        [Key]
        public int UserProfileTvId { get; set; }
        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
        public int? Satallite { get; set; }
        public int? Cable { get; set; }
        public int? Terrestrial { get; set; }
        public int? Internet { get; set; }
    }
}
