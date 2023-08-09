using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model
{
    public class BlockedNumber
    {
        public BlockedNumber()
        {
            Active = true;
        }

        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string TelephoneNumber { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}
