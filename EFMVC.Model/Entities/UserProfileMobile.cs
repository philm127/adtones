using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model
{
    public class UserProfileMobile
    {
        private string _contractType;
        private string _spend;

        [Key]
        public int UserProfileMobileId { get; set; }

        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }

        public string ContractType
        {
            get
            {
                if (string.IsNullOrEmpty(_contractType)) return "0";
                return _contractType;
            }
            set { _contractType = value; }
        }

        public string Spend
        {
            get
            {
                if (string.IsNullOrEmpty(_spend)) return "0";
                return _spend;
            }
            set { _spend = value; }
        }
    }
}