namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangerelationinCampaignmatchUsermatchTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CampaignmatchUsermatches", "UserProfileId", "dbo.UserMatches");
            AddForeignKey("dbo.CampaignmatchUsermatches", "UserProfileId", "dbo.UserProfile", "UserProfileId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CampaignmatchUsermatches", "UserProfileId", "dbo.UserProfile");
            AddForeignKey("dbo.CampaignmatchUsermatches", "UserProfileId", "dbo.UserMatches", "UserProfileId");
        }
    }
}
