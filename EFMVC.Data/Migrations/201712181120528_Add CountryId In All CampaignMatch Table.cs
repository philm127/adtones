namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCountryIdInAllCampaignMatchTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CampaignMatches", "CountryId", c => c.Int());
            AddColumn("dbo.CampaignMatch10", "CountryId", c => c.Int());
            AddColumn("dbo.CampaignMatch2", "CountryId", c => c.Int());
            AddColumn("dbo.CampaignMatch3", "CountryId", c => c.Int());
            AddColumn("dbo.CampaignMatch4", "CountryId", c => c.Int());
            AddColumn("dbo.CampaignMatch5", "CountryId", c => c.Int());
            AddColumn("dbo.CampaignMatch6", "CountryId", c => c.Int());
            AddColumn("dbo.CampaignMatch7", "CountryId", c => c.Int());
            AddColumn("dbo.CampaignMatch8", "CountryId", c => c.Int());
            AddColumn("dbo.CampaignMatch9", "CountryId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CampaignMatch9", "CountryId");
            DropColumn("dbo.CampaignMatch8", "CountryId");
            DropColumn("dbo.CampaignMatch7", "CountryId");
            DropColumn("dbo.CampaignMatch6", "CountryId");
            DropColumn("dbo.CampaignMatch5", "CountryId");
            DropColumn("dbo.CampaignMatch4", "CountryId");
            DropColumn("dbo.CampaignMatch3", "CountryId");
            DropColumn("dbo.CampaignMatch2", "CountryId");
            DropColumn("dbo.CampaignMatch10", "CountryId");
            DropColumn("dbo.CampaignMatches", "CountryId");
        }
    }
}
