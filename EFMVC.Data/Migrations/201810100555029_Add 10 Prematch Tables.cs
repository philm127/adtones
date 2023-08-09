namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add10PrematchTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PreMatch10",
                c => new
                    {
                        PreMatch10Id = c.Int(nullable: false, identity: true),
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
                    })
                .PrimaryKey(t => t.PreMatch10Id);
            
            CreateTable(
                "dbo.PreMatch2",
                c => new
                    {
                        PreMatch2Id = c.Int(nullable: false, identity: true),
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
                    })
                .PrimaryKey(t => t.PreMatch2Id);
            
            CreateTable(
                "dbo.PreMatch3",
                c => new
                    {
                        PreMatch3Id = c.Int(nullable: false, identity: true),
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
                    })
                .PrimaryKey(t => t.PreMatch3Id);
            
            CreateTable(
                "dbo.PreMatch4",
                c => new
                    {
                        PreMatch4Id = c.Int(nullable: false, identity: true),
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
                    })
                .PrimaryKey(t => t.PreMatch4Id);
            
            CreateTable(
                "dbo.PreMatch5",
                c => new
                    {
                        PreMatch5Id = c.Int(nullable: false, identity: true),
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
                    })
                .PrimaryKey(t => t.PreMatch5Id);
            
            CreateTable(
                "dbo.PreMatch6",
                c => new
                    {
                        PreMatch6Id = c.Int(nullable: false, identity: true),
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
                    })
                .PrimaryKey(t => t.PreMatch6Id);
            
            CreateTable(
                "dbo.PreMatch7",
                c => new
                    {
                        PreMatch7Id = c.Int(nullable: false, identity: true),
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
                    })
                .PrimaryKey(t => t.PreMatch7Id);
            
            CreateTable(
                "dbo.PreMatch8",
                c => new
                    {
                        PreMatch8Id = c.Int(nullable: false, identity: true),
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
                    })
                .PrimaryKey(t => t.PreMatch8Id);
            
            CreateTable(
                "dbo.PreMatch9",
                c => new
                    {
                        PreMatch9Id = c.Int(nullable: false, identity: true),
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
                    })
                .PrimaryKey(t => t.PreMatch9Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PreMatch9");
            DropTable("dbo.PreMatch8");
            DropTable("dbo.PreMatch7");
            DropTable("dbo.PreMatch6");
            DropTable("dbo.PreMatch5");
            DropTable("dbo.PreMatch4");
            DropTable("dbo.PreMatch3");
            DropTable("dbo.PreMatch2");
            DropTable("dbo.PreMatch10");
        }
    }
}
