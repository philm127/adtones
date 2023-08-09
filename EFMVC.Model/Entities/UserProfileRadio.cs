using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Collections.Generic;


namespace EFMVC.Model {
    
    public class UserProfileRadio {
        [Key]
        public int UserProfileRadioId { get; set; }
        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
        public int? National { get; set; }
        public int? Local { get; set; }
        public int? Music { get; set; }
        public int? Sport { get; set; }
        public int? Talk { get; set; }
    }
}
