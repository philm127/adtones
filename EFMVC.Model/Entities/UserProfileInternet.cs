using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model
{
    public class UserProfileInternet
    {
        private int? _auctions;
        private int? _research;
        private int? _shopping;
        private int? _socialNetworking;
        private int? _video;

        [Key]
        public int UserProfileInternetId { get; set; }

        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }

        public int? SocialNetworking
        {
            get
            {
                if (_socialNetworking == null) return 0;
                return _socialNetworking;
            }
            set { _socialNetworking = value; }
        }

        public int? Video
        {
            get
            {
                if (_video == null) return 0;
                return _video;
            }
            set { _video = value; }
        }

        public int? Research
        {
            get
            {
                if (_research == null) return 0;
                return _research;
            }
            set { _research = value; }
        }

        public int? Auctions
        {
            get
            {
                if (_auctions == null) return 0;
                return _auctions;
            }
            set { _auctions = value; }
        }

        public int? Shopping
        {
            get
            {
                if (_shopping == null) return 0;
                return _shopping;
            }
            set { _shopping = value; }
        }
    }
}