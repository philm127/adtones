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
    
    public partial class UserProfileMobile
    {
        public int UserProfileMobileId { get; set; }
        public int UserProfileId { get; set; }
        public string ContractType { get; set; }
        public string Spend { get; set; }
    
        public virtual UserProfile UserProfile { get; set; }
    }
}
