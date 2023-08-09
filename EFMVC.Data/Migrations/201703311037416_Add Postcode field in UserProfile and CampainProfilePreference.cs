namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostcodefieldinUserProfileandCampainProfilePreference : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CampaignProfilePreference", "Postcode", c => c.String(maxLength: 100));
            AddColumn("dbo.CampaignProfilePreference", "CountryId", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfile", "Postcode", c => c.String(maxLength: 100));
            CreateIndex("dbo.CampaignProfilePreference", "CountryId");
            //AddForeignKey("dbo.CampaignProfilePreference", "CountryId", "dbo.Country", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CampaignProfilePreference", "CountryId", "dbo.Country");
            DropIndex("dbo.CampaignProfilePreference", new[] { "CountryId" });
            DropColumn("dbo.UserProfile", "Postcode");
            DropColumn("dbo.CampaignProfilePreference", "CountryId");
            DropColumn("dbo.CampaignProfilePreference", "Postcode");
        }
    }
}
