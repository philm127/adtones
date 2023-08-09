namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLinuxServerConnectionStringTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LinuxServerConnectionStrings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OperatorId = c.Int(),
                        ConnectionString = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LinuxServerConnectionStrings");
        }
    }
}
