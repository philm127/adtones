using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model
{
    public class UserProfileAttitude
    {
        private int? _environment;
        private int? _fashion;
        private int? _financialStabiity;
        private int? _fitness;
        private int? _goingOut;
        private int? _holidays;
        private int? _music;
        private int? _religion;

        [Key]
        public int UserProfileAttitudeId { get; set; }

        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }

        public int? Fitness
        {
            get
            {
                if (_fitness == null) return 1;
                return _fitness;
            }
            set { _fitness = value; }
        }

        public int? Holidays
        {
            get
            {
                if (_holidays == null) return 1;
                return _holidays;
            }
            set { _holidays = value; }
        }

        public int? Environment
        {
            get
            {
                if (_environment == null) return 1;
                return _environment;
            }
            set { _environment = value; }
        }

        public int? GoingOut
        {
            get
            {
                if (_goingOut == null) return 1;
                return _goingOut;
            }
            set { _goingOut = value; }
        }

        public int? FinancialStabiity
        {
            get
            {
                if (_financialStabiity == null) return 1;
                return _financialStabiity;
            }
            set { _financialStabiity = value; }
        }

        public int? Religion
        {
            get
            {
                if (_religion == null) return 1;
                return _religion;
            }
            set { _religion = value; }
        }

        public int? Fashion
        {
            get
            {
                if (_fashion == null) return 1;
                return _fashion;
            }
            set { _fashion = value; }
        }

        public int? Music
        {
            get
            {
                if (_music == null) return 1;
                return _music;
            }
            set { _music = value; }
        }
    }
}