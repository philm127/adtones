namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCamapaignMatchRelationship : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PreMatches", "CampaignMatch_CampaignProfileId", "dbo.CampaignMatches");
            DropIndex("dbo.PreMatches", new[] { "CampaignMatch_CampaignProfileId" });
            DropColumn("dbo.PreMatches", "CampaignMatch_CampaignProfileId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PreMatches", "CampaignMatch_CampaignProfileId", c => c.Int());
            CreateIndex("dbo.PreMatches", "CampaignMatch_CampaignProfileId");
            AddForeignKey("dbo.PreMatches", "CampaignMatch_CampaignProfileId", "dbo.CampaignMatches", "CampaignProfileId");
        }
    }
}
