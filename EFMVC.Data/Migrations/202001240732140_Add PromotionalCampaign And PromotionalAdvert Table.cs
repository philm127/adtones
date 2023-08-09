namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPromotionalCampaignAndPromotionalAdvertTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PromotionalAdverts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CampaignID = c.Int(),
                        AdvertName = c.String(),
                        AdvertLocation = c.String(),
                        PromotionalCampaign_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PromotionalCampaigns", t => t.PromotionalCampaign_ID)
                .Index(t => t.PromotionalCampaign_ID);
            
            CreateTable(
                "dbo.PromotionalCampaigns",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OperatorID = c.Int(),
                        CampaignName = c.String(),
                        BatchID = c.Int(nullable: false),
                        MaxDaily = c.Int(nullable: false),
                        MaxWeekly = c.Int(nullable: false),
                        AdvertLocation = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Operators", t => t.OperatorID)
                .Index(t => t.OperatorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PromotionalAdverts", "PromotionalCampaign_ID", "dbo.PromotionalCampaigns");
            DropForeignKey("dbo.PromotionalCampaigns", "OperatorID", "dbo.Operators");
            DropIndex("dbo.PromotionalCampaigns", new[] { "OperatorID" });
            DropIndex("dbo.PromotionalAdverts", new[] { "PromotionalCampaign_ID" });
            DropTable("dbo.PromotionalCampaigns");
            DropTable("dbo.PromotionalAdverts");
        }
    }
}
