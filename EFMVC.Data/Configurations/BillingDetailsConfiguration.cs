using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.Model;

namespace EFMVC.Data.Configurations
{
  public  class BillingDetailsConfiguration : EntityTypeConfiguration<BillingDetails>
    {
        public BillingDetailsConfiguration()
        {
            ToTable("BillingDetails");
            Property(b => b.Id).IsRequired();
            Property(b => b.BillingId).IsRequired();
            Property(b => b.CardType);
            Property(b => b.CardNumber);
            Property(b => b.ExpiryMonth);
            Property(b => b.ExpiryYear);
            Property(b => b.NameOfCard);
            Property(b => b.SecurityCode);
            Property(b => b.BillingAddress);
            Property(b => b.BillingTown);
            Property(b => b.BillingPostcode);
            Property(b => b.PaypalEmail);
            Property(b => b.PaypalTranID);
            
    }
    }
}
