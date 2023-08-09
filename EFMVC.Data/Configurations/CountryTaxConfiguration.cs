using EFMVC.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Data.Configurations
{
   public class CountryTaxConfiguration : EntityTypeConfiguration<CountryTax>
    {
        public CountryTaxConfiguration()
        {
            ToTable("CountryTax");
            Property(b => b.Id).IsRequired();
            Property(b => b.UserId).IsRequired();
            Property(b => b.CountryId).IsRequired();
            Property(b => b.TaxPercantage);
            Property(b => b.CreatedDate);
            Property(b => b.UpdatedDate);
            Property(b => b.Status);
        }
    }
}
