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
    
    public partial class User
    {
        public User()
        {
            this.BlockedNumbers = new HashSet<BlockedNumber>();
            this.UserProfiles = new HashSet<UserProfile>();
        }
    
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PasswordHash { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<System.DateTime> LastLoginTime { get; set; }
        public bool Activated { get; set; }
        public int RoleId { get; set; }
    
        public virtual ICollection<BlockedNumber> BlockedNumbers { get; set; }
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }
}
