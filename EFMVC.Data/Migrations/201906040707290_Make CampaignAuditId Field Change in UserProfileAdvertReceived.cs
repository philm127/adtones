namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeCampaignAuditIdFieldChangeinUserProfileAdvertReceived : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserProfileAdvertsReceiveds", "CampaignAuditId", "dbo.CampaignAudit");
            DropIndex("dbo.UserProfileAdvertsReceiveds", new[] { "CampaignAuditId" });
            AlterColumn("dbo.UserProfileAdvertsReceiveds", "CampaignAuditId", c => c.Int());
            CreateIndex("dbo.UserProfileAdvertsReceiveds", "CampaignAuditId");
            AddForeignKey("dbo.UserProfileAdvertsReceiveds", "CampaignAuditId", "dbo.CampaignAudit", "CampaignAuditId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProfileAdvertsReceiveds", "CampaignAuditId", "dbo.CampaignAudit");
            DropIndex("dbo.UserProfileAdvertsReceiveds", new[] { "CampaignAuditId" });
            AlterColumn("dbo.UserProfileAdvertsReceiveds", "CampaignAuditId", c => c.Int(nullable: false));
            CreateIndex("dbo.UserProfileAdvertsReceiveds", "CampaignAuditId");
            AddForeignKey("dbo.UserProfileAdvertsReceiveds", "CampaignAuditId", "dbo.CampaignAudit", "CampaignAuditId", cascadeDelete: true);
        }
    }
}
