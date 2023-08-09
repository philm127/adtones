namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCampaignConfigtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CampaignConfigs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CampaignID = c.Int(nullable: false),
                        GravityID = c.String(),
                        CampaignText = c.String(),
                        UserText = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CampaignConfigs");
        }
    }
}
