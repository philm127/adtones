using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.Model;


namespace EFMVC.Data.Configurations
{
    public class ContactsConfiguration : EntityTypeConfiguration<Contacts>
    {
        public ContactsConfiguration()
        {
            ToTable("Contacts");
            Property(b => b.Id).IsRequired();
            Property(b => b.UserId).IsRequired();
            Property(b => b.MobileNumber);
            Property(b => b.FixedLine);
            Property(b => b.Email);
            Property(b => b.PhoneNumber);
            Property(b => b.Address);
        }
    }
}
