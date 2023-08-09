using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model
{
    public class UserProfileCinema
    {
        private int? _cinema;

        [Key]
        public int UserProfileCinemaId { get; set; }

        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }

        public int? Cinema
        {
            get
            {
                if (_cinema == null) return 0;
                return _cinema;
            }
            set { _cinema = value; }
        }
    }
}