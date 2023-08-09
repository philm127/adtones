namespace EFMVC.Model
{
    public class UserProfileCreditsReceived
    {
        public int Id { get; set; }
        public int UserProfileId { get; set; }
        public string TotalCredits { get; set; }
        public string LastMonthCredits { get; set; }
        public string CurrentMonthCredits { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }
}