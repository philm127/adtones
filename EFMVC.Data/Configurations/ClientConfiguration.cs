using System.Data.Entity.ModelConfiguration;
using EFMVC.Model;

namespace EFMVC.Data.Configurations
{
    public class ClientConfiguration : EntityTypeConfiguration<Client>
    {
        public ClientConfiguration()
        {
            ToTable("Client");
            Property(b => b.Id).IsRequired();
            Property(b => b.UserId).IsRequired();
            Property(b => b.Name).IsRequired();
            Property(b => b.Description).IsRequired();
            Property(b => b.ContactInfo).IsRequired();
            Property(b => b.Budget).IsRequired();
            Property(b => b.CreatedDate);
            Property(b => b.UpdatedDate);
        }
    }
}
