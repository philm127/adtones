//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EFMVC.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserProfileRadio
    {
        public int UserProfileRadioId { get; set; }
        public int UserProfileId { get; set; }
        public Nullable<int> National { get; set; }
        public Nullable<int> Local { get; set; }
        public Nullable<int> Music { get; set; }
        public Nullable<int> Sport { get; set; }
        public Nullable<int> Talk { get; set; }
    
        public virtual UserProfile UserProfile { get; set; }
    }
}
