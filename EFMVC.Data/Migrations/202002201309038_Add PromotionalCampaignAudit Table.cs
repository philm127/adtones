namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPromotionalCampaignAuditTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PromotionalCampaignAudits",
                c => new
                    {
                        PromotionalCampaignAuditId = c.Int(nullable: false, identity: true),
                        PromotionalCampaignId = c.Int(nullable: false),
                        MSISDN = c.String(),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        PlayLengthTicks = c.Long(nullable: false),
                        DTMFKey = c.Int(nullable: false),
                        AddedDate = c.DateTime(),
                        AdtoneServerPromotionalCampaignId = c.Int(),
                        AdtoneServerDataTransfer = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PromotionalCampaignAuditId)
                .ForeignKey("dbo.PromotionalCampaigns", t => t.PromotionalCampaignId, cascadeDelete: true)
                .Index(t => t.PromotionalCampaignId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PromotionalCampaignAudits", "PromotionalCampaignId", "dbo.PromotionalCampaigns");
            DropIndex("dbo.PromotionalCampaignAudits", new[] { "PromotionalCampaignId" });
            DropTable("dbo.PromotionalCampaignAudits");
        }
    }
}
