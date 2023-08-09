using EFMVC.Data.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.Model;
using System.Data.Entity.ModelConfiguration;

namespace EFMVC.Data.Configurations
{
   public class SystemConfigConfiguration : EntityTypeConfiguration<EFMVC.Model.SystemConfig>
    {
        public SystemConfigConfiguration()
        {
            ToTable("SystemConfig");
            Property(b => b.SystemConfigId).IsRequired();
            Property(b => b.SystemConfigKey);
            Property(b => b.SystemConfigType);
            Property(b => b.SystemConfigValue);
            Property(b => b.CreatedDateTime);
            Property(b => b.UpdatedDateTime);
        }
    }
}
