using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.Model;

namespace EFMVC.Data.Configurations
{
    public class PaymentMethodConfiguration  : EntityTypeConfiguration<PaymentMethod>
    {
        public PaymentMethodConfiguration()
        {
            ToTable("PaymentMethod");
            Property(b => b.Id).IsRequired();
            Property(b => b.Name).IsRequired();
            Property(b => b.Description);
        }
    }
}
