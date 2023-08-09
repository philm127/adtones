namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SystemConfig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SystemConfig",
                c => new
                    {
                        SystemConfigId = c.Int(nullable: false, identity: true),
                        SystemConfigKey = c.String(nullable: false, maxLength: 250),
                        SystemConfigValue = c.String(maxLength: 2000),
                        CreatedDateTime = c.DateTime(nullable: false),
                        UpdatedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SystemConfigId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SystemConfig");
        }
    }
}
