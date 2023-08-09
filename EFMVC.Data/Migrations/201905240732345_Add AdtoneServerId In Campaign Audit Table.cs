namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdtoneServerIdInCampaignAuditTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CampaignAudit", "AdtoneServerCampaignProfileId", c => c.Int());
            AddColumn("dbo.CampaignAudit", "AdtoneServerUserProfileId", c => c.Int());
            AddColumn("dbo.CampaignAudit", "AdtoneServerDataTransfer", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CampaignAudit", "AdtoneServerDataTransfer");
            DropColumn("dbo.CampaignAudit", "AdtoneServerUserProfileId");
            DropColumn("dbo.CampaignAudit", "AdtoneServerCampaignProfileId");
        }
    }
}
