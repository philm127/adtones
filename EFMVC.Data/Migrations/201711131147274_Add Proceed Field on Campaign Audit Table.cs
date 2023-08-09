namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProceedFieldonCampaignAuditTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CampaignAudit", "Proceed", c => c.Boolean(nullable: false));
            AddColumn("dbo.CampaignAudit10", "Proceed", c => c.Boolean(nullable: false));
            AddColumn("dbo.CampaignAudit2", "Proceed", c => c.Boolean(nullable: false));
            AddColumn("dbo.CampaignAudit3", "Proceed", c => c.Boolean(nullable: false));
            AddColumn("dbo.CampaignAudit4", "Proceed", c => c.Boolean(nullable: false));
            AddColumn("dbo.CampaignAudit5", "Proceed", c => c.Boolean(nullable: false));
            AddColumn("dbo.CampaignAudit6", "Proceed", c => c.Boolean(nullable: false));
            AddColumn("dbo.CampaignAudit7", "Proceed", c => c.Boolean(nullable: false));
            AddColumn("dbo.CampaignAudit8", "Proceed", c => c.Boolean(nullable: false));
            AddColumn("dbo.CampaignAudit9", "Proceed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CampaignAudit9", "Proceed");
            DropColumn("dbo.CampaignAudit8", "Proceed");
            DropColumn("dbo.CampaignAudit7", "Proceed");
            DropColumn("dbo.CampaignAudit6", "Proceed");
            DropColumn("dbo.CampaignAudit5", "Proceed");
            DropColumn("dbo.CampaignAudit4", "Proceed");
            DropColumn("dbo.CampaignAudit3", "Proceed");
            DropColumn("dbo.CampaignAudit2", "Proceed");
            DropColumn("dbo.CampaignAudit10", "Proceed");
            DropColumn("dbo.CampaignAudit", "Proceed");
        }
    }
}
