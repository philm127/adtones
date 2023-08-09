namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCampaignmatchUsermatchTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CampaignmatchUsermatches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CampaignProfileId = c.Int(),
                        UserProfileId = c.Int(),
                        MEDIA_URL = c.String(maxLength: 256),
                        BID_VALUE = c.Single(),
                        ADD_START = c.DateTime(),
                        ADD_END = c.DateTime(),
                        DTMF_EVENT = c.String(maxLength: 256),
                        SMS_MESSAGE = c.String(maxLength: 256),
                        EMAIL_MESSAGE = c.String(maxLength: 256),
                        ADD_STATE_ID = c.Int(),
                        MSISDN = c.String(maxLength: 50),
                        EMAIL_ADDRESS = c.String(maxLength: 256),
                        CampaignTime = c.String(maxLength: 10),
                        UserTime = c.String(maxLength: 10),
                        MsUserProfileId = c.String(maxLength: 50),
                        MSCampaignProfileId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CampaignMatches", t => t.CampaignProfileId)
                .ForeignKey("dbo.UserMatches", t => t.UserProfileId)
                .Index(t => t.CampaignProfileId)
                .Index(t => t.UserProfileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CampaignmatchUsermatches", "UserProfileId", "dbo.UserMatches");
            DropForeignKey("dbo.CampaignmatchUsermatches", "CampaignProfileId", "dbo.CampaignMatches");
            DropIndex("dbo.CampaignmatchUsermatches", new[] { "UserProfileId" });
            DropIndex("dbo.CampaignmatchUsermatches", new[] { "CampaignProfileId" });
            DropTable("dbo.CampaignmatchUsermatches");
        }
    }
}
