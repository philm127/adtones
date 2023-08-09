using System.Data.Entity.ModelConfiguration;
using EFMVC.Model;

namespace EFMVC.Data.Configurations
{
    public class UsersCreditConfiguration : EntityTypeConfiguration<UsersCredit>
    {
        public UsersCreditConfiguration()
        {
            ToTable("UsersCredit");
            Property(b => b.Id).IsRequired();
            Property(b => b.UserId).IsRequired();
            Property(b => b.AssignCredit);
            Property(b => b.AvailableCredit);
            Property(b => b.CreatedDate);
            Property(b => b.UpdatedDate);
        }
    }
}
