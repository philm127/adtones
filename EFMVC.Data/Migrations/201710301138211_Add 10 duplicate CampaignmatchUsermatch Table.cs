namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add10duplicateCampaignmatchUsermatchTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CampaignmatchUsermatch10",
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
                .ForeignKey("dbo.CampaignMatch10", t => t.CampaignProfileId)
                .ForeignKey("dbo.UserMatch10", t => t.UserProfileId)
                .Index(t => t.CampaignProfileId)
                .Index(t => t.UserProfileId);
            
            CreateTable(
                "dbo.CampaignmatchUsermatch2",
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
                .ForeignKey("dbo.CampaignMatch2", t => t.CampaignProfileId)
                .ForeignKey("dbo.UserMatch2", t => t.UserProfileId)
                .Index(t => t.CampaignProfileId)
                .Index(t => t.UserProfileId);
            
            CreateTable(
                "dbo.CampaignmatchUsermatch3",
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
                .ForeignKey("dbo.CampaignMatch3", t => t.CampaignProfileId)
                .ForeignKey("dbo.UserMatch3", t => t.UserProfileId)
                .Index(t => t.CampaignProfileId)
                .Index(t => t.UserProfileId);
            
            CreateTable(
                "dbo.CampaignmatchUsermatch4",
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
                .ForeignKey("dbo.CampaignMatch4", t => t.CampaignProfileId)
                .ForeignKey("dbo.UserMatch4", t => t.UserProfileId)
                .Index(t => t.CampaignProfileId)
                .Index(t => t.UserProfileId);
            
            CreateTable(
                "dbo.CampaignmatchUsermatch5",
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
                .ForeignKey("dbo.CampaignMatch5", t => t.CampaignProfileId)
                .ForeignKey("dbo.UserMatch5", t => t.UserProfileId)
                .Index(t => t.CampaignProfileId)
                .Index(t => t.UserProfileId);
            
            CreateTable(
                "dbo.CampaignmatchUsermatch6",
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
                .ForeignKey("dbo.CampaignMatch6", t => t.CampaignProfileId)
                .ForeignKey("dbo.UserMatch6", t => t.UserProfileId)
                .Index(t => t.CampaignProfileId)
                .Index(t => t.UserProfileId);
            
            CreateTable(
                "dbo.CampaignmatchUsermatch7",
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
                .ForeignKey("dbo.CampaignMatch7", t => t.CampaignProfileId)
                .ForeignKey("dbo.UserMatch7", t => t.UserProfileId)
                .Index(t => t.CampaignProfileId)
                .Index(t => t.UserProfileId);
            
            CreateTable(
                "dbo.CampaignmatchUsermatch8",
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
                .ForeignKey("dbo.CampaignMatch8", t => t.CampaignProfileId)
                .ForeignKey("dbo.UserMatch8", t => t.UserProfileId)
                .Index(t => t.CampaignProfileId)
                .Index(t => t.UserProfileId);
            
            CreateTable(
                "dbo.CampaignmatchUsermatch9",
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
                .ForeignKey("dbo.CampaignMatch9", t => t.CampaignProfileId)
                .ForeignKey("dbo.UserMatch9", t => t.UserProfileId)
                .Index(t => t.CampaignProfileId)
                .Index(t => t.UserProfileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CampaignmatchUsermatch9", "UserProfileId", "dbo.UserMatch9");
            DropForeignKey("dbo.CampaignmatchUsermatch9", "CampaignProfileId", "dbo.CampaignMatch9");
            DropForeignKey("dbo.CampaignmatchUsermatch8", "UserProfileId", "dbo.UserMatch8");
            DropForeignKey("dbo.CampaignmatchUsermatch8", "CampaignProfileId", "dbo.CampaignMatch8");
            DropForeignKey("dbo.CampaignmatchUsermatch7", "UserProfileId", "dbo.UserMatch7");
            DropForeignKey("dbo.CampaignmatchUsermatch7", "CampaignProfileId", "dbo.CampaignMatch7");
            DropForeignKey("dbo.CampaignmatchUsermatch6", "UserProfileId", "dbo.UserMatch6");
            DropForeignKey("dbo.CampaignmatchUsermatch6", "CampaignProfileId", "dbo.CampaignMatch6");
            DropForeignKey("dbo.CampaignmatchUsermatch5", "UserProfileId", "dbo.UserMatch5");
            DropForeignKey("dbo.CampaignmatchUsermatch5", "CampaignProfileId", "dbo.CampaignMatch5");
            DropForeignKey("dbo.CampaignmatchUsermatch4", "UserProfileId", "dbo.UserMatch4");
            DropForeignKey("dbo.CampaignmatchUsermatch4", "CampaignProfileId", "dbo.CampaignMatch4");
            DropForeignKey("dbo.CampaignmatchUsermatch3", "UserProfileId", "dbo.UserMatch3");
            DropForeignKey("dbo.CampaignmatchUsermatch3", "CampaignProfileId", "dbo.CampaignMatch3");
            DropForeignKey("dbo.CampaignmatchUsermatch2", "UserProfileId", "dbo.UserMatch2");
            DropForeignKey("dbo.CampaignmatchUsermatch2", "CampaignProfileId", "dbo.CampaignMatch2");
            DropForeignKey("dbo.CampaignmatchUsermatch10", "UserProfileId", "dbo.UserMatch10");
            DropForeignKey("dbo.CampaignmatchUsermatch10", "CampaignProfileId", "dbo.CampaignMatch10");
            DropIndex("dbo.CampaignmatchUsermatch9", new[] { "UserProfileId" });
            DropIndex("dbo.CampaignmatchUsermatch9", new[] { "CampaignProfileId" });
            DropIndex("dbo.CampaignmatchUsermatch8", new[] { "UserProfileId" });
            DropIndex("dbo.CampaignmatchUsermatch8", new[] { "CampaignProfileId" });
            DropIndex("dbo.CampaignmatchUsermatch7", new[] { "UserProfileId" });
            DropIndex("dbo.CampaignmatchUsermatch7", new[] { "CampaignProfileId" });
            DropIndex("dbo.CampaignmatchUsermatch6", new[] { "UserProfileId" });
            DropIndex("dbo.CampaignmatchUsermatch6", new[] { "CampaignProfileId" });
            DropIndex("dbo.CampaignmatchUsermatch5", new[] { "UserProfileId" });
            DropIndex("dbo.CampaignmatchUsermatch5", new[] { "CampaignProfileId" });
            DropIndex("dbo.CampaignmatchUsermatch4", new[] { "UserProfileId" });
            DropIndex("dbo.CampaignmatchUsermatch4", new[] { "CampaignProfileId" });
            DropIndex("dbo.CampaignmatchUsermatch3", new[] { "UserProfileId" });
            DropIndex("dbo.CampaignmatchUsermatch3", new[] { "CampaignProfileId" });
            DropIndex("dbo.CampaignmatchUsermatch2", new[] { "UserProfileId" });
            DropIndex("dbo.CampaignmatchUsermatch2", new[] { "CampaignProfileId" });
            DropIndex("dbo.CampaignmatchUsermatch10", new[] { "UserProfileId" });
            DropIndex("dbo.CampaignmatchUsermatch10", new[] { "CampaignProfileId" });
            DropTable("dbo.CampaignmatchUsermatch9");
            DropTable("dbo.CampaignmatchUsermatch8");
            DropTable("dbo.CampaignmatchUsermatch7");
            DropTable("dbo.CampaignmatchUsermatch6");
            DropTable("dbo.CampaignmatchUsermatch5");
            DropTable("dbo.CampaignmatchUsermatch4");
            DropTable("dbo.CampaignmatchUsermatch3");
            DropTable("dbo.CampaignmatchUsermatch2");
            DropTable("dbo.CampaignmatchUsermatch10");
        }
    }
}
