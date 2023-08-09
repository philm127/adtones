namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCampaignCreditPeriodTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CampaignCreditPeriods",
                c => new
                    {
                        CampaignCreditPeriodId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CampaignProfileId = c.Int(nullable: false),
                        CreditPeriod = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                        AdtoneServerCampaignCreditPeriodId = c.Int(),
                    })
                .PrimaryKey(t => t.CampaignCreditPeriodId)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CampaignProfileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CampaignCreditPeriods", "UserId", "dbo.Users");
            DropForeignKey("dbo.CampaignCreditPeriods", "CampaignProfileId", "dbo.CampaignProfile");
            DropIndex("dbo.CampaignCreditPeriods", new[] { "CampaignProfileId" });
            DropIndex("dbo.CampaignCreditPeriods", new[] { "UserId" });
            DropTable("dbo.CampaignCreditPeriods");
        }
    }
}
