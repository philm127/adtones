using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.Model;

namespace EFMVC.Data.Configurations
{
    public class BillingConfiguration : EntityTypeConfiguration<Billing>
    {
        public BillingConfiguration()
        {
            ToTable("Billing");
            Property(b => b.Id).IsRequired();
            Property(b => b.UserId).IsRequired();
            Property(b => b.ClientId).IsRequired();
            Property(b => b.CampaignProfileId).IsRequired();
            Property(b => b.PaymentMethodId).IsRequired();
            Property(b => b.InvoiceNumber).IsRequired();
            Property(b => b.PONumber);
            Property(b => b.FundAmount).HasPrecision(18, 6);
            Property(b => b.TaxPercantage).HasPrecision(18, 6);
            Property(b => b.TotalAmount).HasPrecision(18, 6);
            Property(b => b.PaymentDate);
            Property(b => b.SettledDate);
            Property(b => b.Status);
            Property(b => b.ErrorCode);
            Property(b => b.ErrorDescription);
        }
    }
}
