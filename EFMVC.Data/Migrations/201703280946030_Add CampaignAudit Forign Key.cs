namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCampaignAuditForignKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfileAdvertsReceiveds", "CampaignAuditId", c => c.Int(nullable: false));
            CreateIndex("dbo.UserProfileAdvertsReceiveds", "CampaignAuditId");
           // AddForeignKey("dbo.UserProfileAdvertsReceiveds", "CampaignAuditId", "dbo.CampaignAudit", "CampaignAuditId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.UserProfileAdvertsReceiveds", "CampaignAuditId", "dbo.CampaignAudit");
            //DropIndex("dbo.UserProfileAdvertsReceiveds", new[] { "CampaignAuditId" });
            //DropColumn("dbo.UserProfileAdvertsReceiveds", "CampaignAuditId");
        }
    }
}
