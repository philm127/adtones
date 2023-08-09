namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPreMatchtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PreMatches",
                c => new
                    {
                        PreMatchId = c.Int(nullable: false, identity: true),
                        CampaignMatchId = c.Int(),
                        MEDIA_URL = c.String(maxLength: 256),
                        BID_VALUE = c.Single(),
                        SMS_MESSAGE = c.String(maxLength: 256),
                        EMAIL_MESSAGE = c.String(maxLength: 256),
                        ADD_STATE_ID = c.Int(),
                        MSISDN = c.String(maxLength: 50),
                        EMAIL_ADDRESS = c.String(maxLength: 256),
                        MsUserProfileId = c.String(maxLength: 50),
                        MSCampaignProfileId = c.String(maxLength: 50),
                        CampaignMatch_CampaignProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.PreMatchId)
                .ForeignKey("dbo.CampaignMatches", t => t.CampaignMatch_CampaignProfileId)
                .Index(t => t.CampaignMatch_CampaignProfileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PreMatches", "CampaignMatch_CampaignProfileId", "dbo.CampaignMatches");
            DropIndex("dbo.PreMatches", new[] { "CampaignMatch_CampaignProfileId" });
            DropTable("dbo.PreMatches");
        }
    }
}
