using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EFMVC.Core.Common;
using EFMVC.Model.Entities;

namespace EFMVC.Model
{
    public class User
    {
        public User()
        {
            this.Adverts = new HashSet<Advert>();
            this.BlockedNumbers = new HashSet<BlockedNumber>();
            this.CampaignProfiles = new HashSet<CampaignProfile>();
            this.UserProfiles = new HashSet<UserProfile>();
            this.Clients = new HashSet<Client>();
            this.Questions = new HashSet<Question>();           
        }

        [Key]
        [Required]
        public int UserId { get; set; }

        public int OperatorId { get; set; }
        public virtual Operator Operator { get; set; }

       // [Required] Code commented to 14/09/2018 to make email optional 
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string PasswordHash { get; set; }
        public System.DateTime DateCreated { get; set; }
      
        public string Organisation { get; set; }
        public Nullable<System.DateTime> LastLoginTime { get; set; }
        public int Activated { get; set; }
        public int RoleId { get; set; }
        public bool VerificationStatus { get; set; }

        public int Outstandingdays { get; set; }
        public bool IsMsisdnMatch { get; set; }
        public bool IsEmailVerfication { get; set; }
        public string PhoneticAlphabet { get; set; }
        public bool IsMobileVerfication { get; set; }

        public int? OrganisationTypeId { get; set; }
        public virtual OrganisationType OrganisationTypes { get; set; }

        [StringLength(50)]
        public string UserMatchTableName { get; set; }

        public int? AdtoneServerUserId { get; set; }
        [StringLength(200)]
        public string  TibcoMessageId { get; set; }

        public bool IsSessionFlag { get; set; }
        public Nullable<System.DateTime> LockOutTime { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        public virtual ICollection<Advert> Adverts { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<BlockedNumber> BlockedNumbers { get; set; }
        public virtual ICollection<CampaignProfile> CampaignProfiles { get; set; }
        public virtual ICollection<UsersCredit> UsersCredit { get; set; }
        
        public virtual ICollection<UsersCreditPayment> UsersCreditPayment { get; set; }
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public UserProfile GetUserProfile
        {
            get { return UserProfiles.FirstOrDefault(); }
        }

        public string Password
        {
            set { PasswordHash = Md5Encrypt.Md5EncryptPassword(value); }
        }       

        internal static string GenerateRandomString()
        {
            var builder = new StringBuilder();
            var random = new Random();
            for (int i = 0; i < 6; i++)
            {
                char ch = Convert.ToChar(Convert.ToInt32(Math.Floor(25 * random.NextDouble() + 75)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        public string ResetPassword()
        {
            var newPass = GenerateRandomString();
            Password = newPass;
            return newPass;
        }

        public string DisplayName
        {
            get { return FirstName; }
        }

       
    }
}
