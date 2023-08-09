using EFMVC.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Data.Configurations
{
    public class CountryConfiguration : EntityTypeConfiguration<Country>
    {
        public CountryConfiguration()
        {
            ToTable("Country");
            Property(b => b.Id).IsRequired();
            Property(b => b.UserId).IsRequired();
            Property(b => b.Name);
            Property(b => b.ShortName);
            Property(b => b.CreatedDate);
            Property(b => b.UpdatedDate);
            Property(b => b.Status);
        }
    }
}
