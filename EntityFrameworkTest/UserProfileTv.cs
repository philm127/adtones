//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EntityFrameworkTest
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserProfileTv
    {
        public int UserProfileTvId { get; set; }
        public int UserProfileId { get; set; }
        public Nullable<int> Satallite { get; set; }
        public Nullable<int> Cable { get; set; }
        public Nullable<int> Terrestrial { get; set; }
        public Nullable<int> Internet { get; set; }
    
        public virtual UserProfile UserProfile { get; set; }
    }
}
