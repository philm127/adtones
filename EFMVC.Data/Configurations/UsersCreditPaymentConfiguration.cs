using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.Model;

namespace EFMVC.Data.Configurations
{
    public class UsersCreditPaymentConfiguration : EntityTypeConfiguration<UsersCreditPayment>
    {
        public UsersCreditPaymentConfiguration()
        {
            ToTable("UsersCreditPayment");
            Property(b => b.Id).IsRequired();
            Property(b => b.UserId).IsRequired();
            Property(b => b.BillingId).IsRequired();
            Property(b => b.Amount);
            Property(b => b.Description);
            Property(b => b.Status);
            Property(b => b.CreatedDate);
            Property(b => b.UpdatedDate);
            HasRequired(p => p.User)
         .WithMany(c => c.UsersCreditPayment)
         .HasForeignKey(p => p.UserId);

            HasRequired(p => p.Billing)
       .WithMany(c => c.UsersCreditPayment)
       .HasForeignKey(p => p.BillingId);
        }
    }
}
