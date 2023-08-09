using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.Model;


namespace EFMVC.Data.Configurations
{
    public class CompanyDetailsConfiguration : EntityTypeConfiguration<CompanyDetails>
    {
        public CompanyDetailsConfiguration()
        {
            ToTable("CompanyDetails");
            Property(b => b.Id).IsRequired();
            Property(b => b.UserId).IsRequired();
            Property(b => b.CompanyName);
            Property(b => b.Address);
            Property(b => b.AdditionalAddress);
            Property(b => b.Town);
            Property(b => b.PostCode);
            Property(b => b.CountryId).IsRequired();
        }
    }
}
