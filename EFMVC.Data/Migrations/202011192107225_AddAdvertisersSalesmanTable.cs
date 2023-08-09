namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdvertisersSalesmanTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Advertisers_SalesTeam",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AdvertiserId = c.Int(nullable: false),
                        SalesExecId = c.Int(nullable: false),
                        SalesManId = c.Int(nullable: false),
                        MailSupressed = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Advertisers_SalesTeam");
        }
    }
}
