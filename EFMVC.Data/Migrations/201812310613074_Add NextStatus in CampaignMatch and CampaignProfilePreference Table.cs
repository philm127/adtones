namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNextStatusinCampaignMatchandCampaignProfilePreferenceTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CampaignProfilePreference", "NextStatus", c => c.Boolean(nullable: false));
            AddColumn("dbo.CampaignMatches", "NextStatus", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CampaignMatches", "NextStatus");
            DropColumn("dbo.CampaignProfilePreference", "NextStatus");
        }
    }
}
