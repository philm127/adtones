namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addaddeddateincampaignaudittable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CampaignAudit", "AddedDate", c => c.DateTime());
            AddColumn("dbo.UserProfileAdvertsReceiveds", "AddedDate", c => c.DateTime());
            AddColumn("dbo.UserProfileAdvertsReceived10", "Proceed", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfileAdvertsReceived10", "AddedDate", c => c.DateTime());
            AddColumn("dbo.CampaignAudit10", "AddedDate", c => c.DateTime());
            AddColumn("dbo.UserProfileAdvertsReceived2", "Proceed", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfileAdvertsReceived2", "AddedDate", c => c.DateTime());
            AddColumn("dbo.CampaignAudit2", "AddedDate", c => c.DateTime());
            AddColumn("dbo.UserProfileAdvertsReceived3", "Proceed", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfileAdvertsReceived3", "AddedDate", c => c.DateTime());
            AddColumn("dbo.CampaignAudit3", "AddedDate", c => c.DateTime());
            AddColumn("dbo.UserProfileAdvertsReceived4", "Proceed", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfileAdvertsReceived4", "AddedDate", c => c.DateTime());
            AddColumn("dbo.CampaignAudit4", "AddedDate", c => c.DateTime());
            AddColumn("dbo.UserProfileAdvertsReceived5", "Proceed", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfileAdvertsReceived5", "AddedDate", c => c.DateTime());
            AddColumn("dbo.CampaignAudit5", "AddedDate", c => c.DateTime());
            AddColumn("dbo.UserProfileAdvertsReceived6", "Proceed", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfileAdvertsReceived6", "AddedDate", c => c.DateTime());
            AddColumn("dbo.CampaignAudit6", "AddedDate", c => c.DateTime());
            AddColumn("dbo.UserProfileAdvertsReceived7", "Proceed", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfileAdvertsReceived7", "AddedDate", c => c.DateTime());
            AddColumn("dbo.CampaignAudit7", "AddedDate", c => c.DateTime());
            AddColumn("dbo.UserProfileAdvertsReceived8", "Proceed", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfileAdvertsReceived8", "AddedDate", c => c.DateTime());
            AddColumn("dbo.CampaignAudit8", "AddedDate", c => c.DateTime());
            AddColumn("dbo.UserProfileAdvertsReceived9", "Proceed", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfileAdvertsReceived9", "AddedDate", c => c.DateTime());
            AddColumn("dbo.CampaignAudit9", "AddedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CampaignAudit9", "AddedDate");
            DropColumn("dbo.UserProfileAdvertsReceived9", "AddedDate");
            DropColumn("dbo.UserProfileAdvertsReceived9", "Proceed");
            DropColumn("dbo.CampaignAudit8", "AddedDate");
            DropColumn("dbo.UserProfileAdvertsReceived8", "AddedDate");
            DropColumn("dbo.UserProfileAdvertsReceived8", "Proceed");
            DropColumn("dbo.CampaignAudit7", "AddedDate");
            DropColumn("dbo.UserProfileAdvertsReceived7", "AddedDate");
            DropColumn("dbo.UserProfileAdvertsReceived7", "Proceed");
            DropColumn("dbo.CampaignAudit6", "AddedDate");
            DropColumn("dbo.UserProfileAdvertsReceived6", "AddedDate");
            DropColumn("dbo.UserProfileAdvertsReceived6", "Proceed");
            DropColumn("dbo.CampaignAudit5", "AddedDate");
            DropColumn("dbo.UserProfileAdvertsReceived5", "AddedDate");
            DropColumn("dbo.UserProfileAdvertsReceived5", "Proceed");
            DropColumn("dbo.CampaignAudit4", "AddedDate");
            DropColumn("dbo.UserProfileAdvertsReceived4", "AddedDate");
            DropColumn("dbo.UserProfileAdvertsReceived4", "Proceed");
            DropColumn("dbo.CampaignAudit3", "AddedDate");
            DropColumn("dbo.UserProfileAdvertsReceived3", "AddedDate");
            DropColumn("dbo.UserProfileAdvertsReceived3", "Proceed");
            DropColumn("dbo.CampaignAudit2", "AddedDate");
            DropColumn("dbo.UserProfileAdvertsReceived2", "AddedDate");
            DropColumn("dbo.UserProfileAdvertsReceived2", "Proceed");
            DropColumn("dbo.CampaignAudit10", "AddedDate");
            DropColumn("dbo.UserProfileAdvertsReceived10", "AddedDate");
            DropColumn("dbo.UserProfileAdvertsReceived10", "Proceed");
            DropColumn("dbo.UserProfileAdvertsReceiveds", "AddedDate");
            DropColumn("dbo.CampaignAudit", "AddedDate");
        }
    }
}
