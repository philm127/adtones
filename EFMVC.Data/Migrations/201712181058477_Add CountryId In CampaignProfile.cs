namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCountryIdInCampaignProfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CampaignProfile", "CountryId", c => c.Int());
            CreateIndex("dbo.CampaignProfile", "CountryId");
            AddForeignKey("dbo.CampaignProfile", "CountryId", "dbo.Country", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CampaignProfile", "CountryId", "dbo.Country");
            DropIndex("dbo.CampaignProfile", new[] { "CountryId" });
            DropColumn("dbo.CampaignProfile", "CountryId");
        }
    }
}
