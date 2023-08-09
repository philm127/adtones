namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCampaignProfileIdinImportstable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Imports", "CampaignProfileId", c => c.Int(nullable: false));
            AddColumn("dbo.Import10", "CampaignProfileId", c => c.Int(nullable: false));
            AddColumn("dbo.Import2", "CampaignProfileId", c => c.Int(nullable: false));
            AddColumn("dbo.Import3", "CampaignProfileId", c => c.Int(nullable: false));
            AddColumn("dbo.Import4", "CampaignProfileId", c => c.Int(nullable: false));
            AddColumn("dbo.Import5", "CampaignProfileId", c => c.Int(nullable: false));
            AddColumn("dbo.Import6", "CampaignProfileId", c => c.Int(nullable: false));
            AddColumn("dbo.Import7", "CampaignProfileId", c => c.Int(nullable: false));
            AddColumn("dbo.Import8", "CampaignProfileId", c => c.Int(nullable: false));
            AddColumn("dbo.Import9", "CampaignProfileId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Import9", "CampaignProfileId");
            DropColumn("dbo.Import8", "CampaignProfileId");
            DropColumn("dbo.Import7", "CampaignProfileId");
            DropColumn("dbo.Import6", "CampaignProfileId");
            DropColumn("dbo.Import5", "CampaignProfileId");
            DropColumn("dbo.Import4", "CampaignProfileId");
            DropColumn("dbo.Import3", "CampaignProfileId");
            DropColumn("dbo.Import2", "CampaignProfileId");
            DropColumn("dbo.Import10", "CampaignProfileId");
            DropColumn("dbo.Imports", "CampaignProfileId");
        }
    }
}
