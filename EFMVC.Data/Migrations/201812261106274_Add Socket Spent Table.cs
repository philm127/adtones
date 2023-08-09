namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSocketSpentTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SocketSpentBucket10",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_BATCH_ID = c.Int(),
                        MSISDN = c.String(maxLength: 50),
                        ITEM_TOTAL = c.Int(),
                        EMAIL_ADDRESS = c.String(maxLength: 255),
                        MsUserProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SocketSpentBucket2",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_BATCH_ID = c.Int(),
                        MSISDN = c.String(maxLength: 50),
                        ITEM_TOTAL = c.Int(),
                        EMAIL_ADDRESS = c.String(maxLength: 255),
                        MsUserProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SocketSpentBucket3",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_BATCH_ID = c.Int(),
                        MSISDN = c.String(maxLength: 50),
                        ITEM_TOTAL = c.Int(),
                        EMAIL_ADDRESS = c.String(maxLength: 255),
                        MsUserProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SocketSpentBucket4",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_BATCH_ID = c.Int(),
                        MSISDN = c.String(maxLength: 50),
                        ITEM_TOTAL = c.Int(),
                        EMAIL_ADDRESS = c.String(maxLength: 255),
                        MsUserProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SocketSpentBucket5",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_BATCH_ID = c.Int(),
                        MSISDN = c.String(maxLength: 50),
                        ITEM_TOTAL = c.Int(),
                        EMAIL_ADDRESS = c.String(maxLength: 255),
                        MsUserProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SocketSpentBucket6",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_BATCH_ID = c.Int(),
                        MSISDN = c.String(maxLength: 50),
                        ITEM_TOTAL = c.Int(),
                        EMAIL_ADDRESS = c.String(maxLength: 255),
                        MsUserProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SocketSpentBucket7",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_BATCH_ID = c.Int(),
                        MSISDN = c.String(maxLength: 50),
                        ITEM_TOTAL = c.Int(),
                        EMAIL_ADDRESS = c.String(maxLength: 255),
                        MsUserProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SocketSpentBucket8",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_BATCH_ID = c.Int(),
                        MSISDN = c.String(maxLength: 50),
                        ITEM_TOTAL = c.Int(),
                        EMAIL_ADDRESS = c.String(maxLength: 255),
                        MsUserProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SocketSpentBucket9",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_BATCH_ID = c.Int(),
                        MSISDN = c.String(maxLength: 50),
                        ITEM_TOTAL = c.Int(),
                        EMAIL_ADDRESS = c.String(maxLength: 255),
                        MsUserProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SocketSpentBucketItem10",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_ID = c.Int(),
                        PRIORITY = c.Int(),
                        ADD_STATE_ID = c.Int(),
                        MEDIA_URL = c.String(maxLength: 256),
                        BID_VALUE = c.Single(),
                        ADD_START = c.DateTime(),
                        ADD_END = c.DateTime(),
                        CAMPAIGNID = c.String(maxLength: 256),
                        DTMF_EVENT = c.String(maxLength: 256),
                        SMS_MESSAGE = c.String(maxLength: 256),
                        EMAIL_MESSAGE = c.String(maxLength: 256),
                        ORIGINATOR = c.String(maxLength: 256),
                        Processed = c.Boolean(),
                        PlayLengthTicks = c.Int(),
                        SMSCost = c.Single(),
                        EmailCost = c.Single(),
                        TotalCost = c.Single(),
                        MSCampaignProfileId = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SocketSpentBucket10", t => t.BUCKET_ID)
                .Index(t => t.BUCKET_ID);
            
            CreateTable(
                "dbo.SocketSpentBucketItem2",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_ID = c.Int(),
                        PRIORITY = c.Int(),
                        ADD_STATE_ID = c.Int(),
                        MEDIA_URL = c.String(maxLength: 256),
                        BID_VALUE = c.Single(),
                        ADD_START = c.DateTime(),
                        ADD_END = c.DateTime(),
                        CAMPAIGNID = c.String(maxLength: 256),
                        DTMF_EVENT = c.String(maxLength: 256),
                        SMS_MESSAGE = c.String(maxLength: 256),
                        EMAIL_MESSAGE = c.String(maxLength: 256),
                        ORIGINATOR = c.String(maxLength: 256),
                        Processed = c.Boolean(),
                        PlayLengthTicks = c.Int(),
                        SMSCost = c.Single(),
                        EmailCost = c.Single(),
                        TotalCost = c.Single(),
                        MSCampaignProfileId = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SocketSpentBucket2", t => t.BUCKET_ID)
                .Index(t => t.BUCKET_ID);
            
            CreateTable(
                "dbo.SocketSpentBucketItem3",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_ID = c.Int(),
                        PRIORITY = c.Int(),
                        ADD_STATE_ID = c.Int(),
                        MEDIA_URL = c.String(maxLength: 256),
                        BID_VALUE = c.Single(),
                        ADD_START = c.DateTime(),
                        ADD_END = c.DateTime(),
                        CAMPAIGNID = c.String(maxLength: 256),
                        DTMF_EVENT = c.String(maxLength: 256),
                        SMS_MESSAGE = c.String(maxLength: 256),
                        EMAIL_MESSAGE = c.String(maxLength: 256),
                        ORIGINATOR = c.String(maxLength: 256),
                        Processed = c.Boolean(),
                        PlayLengthTicks = c.Int(),
                        SMSCost = c.Single(),
                        EmailCost = c.Single(),
                        TotalCost = c.Single(),
                        MSCampaignProfileId = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SocketSpentBucket3", t => t.BUCKET_ID)
                .Index(t => t.BUCKET_ID);
            
            CreateTable(
                "dbo.SocketSpentBucketItem4",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_ID = c.Int(),
                        PRIORITY = c.Int(),
                        ADD_STATE_ID = c.Int(),
                        MEDIA_URL = c.String(maxLength: 256),
                        BID_VALUE = c.Single(),
                        ADD_START = c.DateTime(),
                        ADD_END = c.DateTime(),
                        CAMPAIGNID = c.String(maxLength: 256),
                        DTMF_EVENT = c.String(maxLength: 256),
                        SMS_MESSAGE = c.String(maxLength: 256),
                        EMAIL_MESSAGE = c.String(maxLength: 256),
                        ORIGINATOR = c.String(maxLength: 256),
                        Processed = c.Boolean(),
                        PlayLengthTicks = c.Int(),
                        SMSCost = c.Single(),
                        EmailCost = c.Single(),
                        TotalCost = c.Single(),
                        MSCampaignProfileId = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SocketSpentBucket4", t => t.BUCKET_ID)
                .Index(t => t.BUCKET_ID);
            
            CreateTable(
                "dbo.SocketSpentBucketItem5",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_ID = c.Int(),
                        PRIORITY = c.Int(),
                        ADD_STATE_ID = c.Int(),
                        MEDIA_URL = c.String(maxLength: 256),
                        BID_VALUE = c.Single(),
                        ADD_START = c.DateTime(),
                        ADD_END = c.DateTime(),
                        CAMPAIGNID = c.String(maxLength: 256),
                        DTMF_EVENT = c.String(maxLength: 256),
                        SMS_MESSAGE = c.String(maxLength: 256),
                        EMAIL_MESSAGE = c.String(maxLength: 256),
                        ORIGINATOR = c.String(maxLength: 256),
                        Processed = c.Boolean(),
                        PlayLengthTicks = c.Int(),
                        SMSCost = c.Single(),
                        EmailCost = c.Single(),
                        TotalCost = c.Single(),
                        MSCampaignProfileId = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SocketSpentBucket5", t => t.BUCKET_ID)
                .Index(t => t.BUCKET_ID);
            
            CreateTable(
                "dbo.SocketSpentBucketItem6",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_ID = c.Int(),
                        PRIORITY = c.Int(),
                        ADD_STATE_ID = c.Int(),
                        MEDIA_URL = c.String(maxLength: 256),
                        BID_VALUE = c.Single(),
                        ADD_START = c.DateTime(),
                        ADD_END = c.DateTime(),
                        CAMPAIGNID = c.String(maxLength: 256),
                        DTMF_EVENT = c.String(maxLength: 256),
                        SMS_MESSAGE = c.String(maxLength: 256),
                        EMAIL_MESSAGE = c.String(maxLength: 256),
                        ORIGINATOR = c.String(maxLength: 256),
                        Processed = c.Boolean(),
                        PlayLengthTicks = c.Int(),
                        SMSCost = c.Single(),
                        EmailCost = c.Single(),
                        TotalCost = c.Single(),
                        MSCampaignProfileId = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SocketSpentBucket6", t => t.BUCKET_ID)
                .Index(t => t.BUCKET_ID);
            
            CreateTable(
                "dbo.SocketSpentBucketItem7",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_ID = c.Int(),
                        PRIORITY = c.Int(),
                        ADD_STATE_ID = c.Int(),
                        MEDIA_URL = c.String(maxLength: 256),
                        BID_VALUE = c.Single(),
                        ADD_START = c.DateTime(),
                        ADD_END = c.DateTime(),
                        CAMPAIGNID = c.String(maxLength: 256),
                        DTMF_EVENT = c.String(maxLength: 256),
                        SMS_MESSAGE = c.String(maxLength: 256),
                        EMAIL_MESSAGE = c.String(maxLength: 256),
                        ORIGINATOR = c.String(maxLength: 256),
                        Processed = c.Boolean(),
                        PlayLengthTicks = c.Int(),
                        SMSCost = c.Single(),
                        EmailCost = c.Single(),
                        TotalCost = c.Single(),
                        MSCampaignProfileId = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SocketSpentBucket7", t => t.BUCKET_ID)
                .Index(t => t.BUCKET_ID);
            
            CreateTable(
                "dbo.SocketSpentBucketItem8",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_ID = c.Int(),
                        PRIORITY = c.Int(),
                        ADD_STATE_ID = c.Int(),
                        MEDIA_URL = c.String(maxLength: 256),
                        BID_VALUE = c.Single(),
                        ADD_START = c.DateTime(),
                        ADD_END = c.DateTime(),
                        CAMPAIGNID = c.String(maxLength: 256),
                        DTMF_EVENT = c.String(maxLength: 256),
                        SMS_MESSAGE = c.String(maxLength: 256),
                        EMAIL_MESSAGE = c.String(maxLength: 256),
                        ORIGINATOR = c.String(maxLength: 256),
                        Processed = c.Boolean(),
                        PlayLengthTicks = c.Int(),
                        SMSCost = c.Single(),
                        EmailCost = c.Single(),
                        TotalCost = c.Single(),
                        MSCampaignProfileId = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SocketSpentBucket8", t => t.BUCKET_ID)
                .Index(t => t.BUCKET_ID);
            
            CreateTable(
                "dbo.SocketSpentBucketItem9",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_ID = c.Int(),
                        PRIORITY = c.Int(),
                        ADD_STATE_ID = c.Int(),
                        MEDIA_URL = c.String(maxLength: 256),
                        BID_VALUE = c.Single(),
                        ADD_START = c.DateTime(),
                        ADD_END = c.DateTime(),
                        CAMPAIGNID = c.String(maxLength: 256),
                        DTMF_EVENT = c.String(maxLength: 256),
                        SMS_MESSAGE = c.String(maxLength: 256),
                        EMAIL_MESSAGE = c.String(maxLength: 256),
                        ORIGINATOR = c.String(maxLength: 256),
                        Processed = c.Boolean(),
                        PlayLengthTicks = c.Int(),
                        SMSCost = c.Single(),
                        EmailCost = c.Single(),
                        TotalCost = c.Single(),
                        MSCampaignProfileId = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SocketSpentBucket9", t => t.BUCKET_ID)
                .Index(t => t.BUCKET_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SocketSpentBucketItem9", "BUCKET_ID", "dbo.SocketSpentBucket9");
            DropForeignKey("dbo.SocketSpentBucketItem8", "BUCKET_ID", "dbo.SocketSpentBucket8");
            DropForeignKey("dbo.SocketSpentBucketItem7", "BUCKET_ID", "dbo.SocketSpentBucket7");
            DropForeignKey("dbo.SocketSpentBucketItem6", "BUCKET_ID", "dbo.SocketSpentBucket6");
            DropForeignKey("dbo.SocketSpentBucketItem5", "BUCKET_ID", "dbo.SocketSpentBucket5");
            DropForeignKey("dbo.SocketSpentBucketItem4", "BUCKET_ID", "dbo.SocketSpentBucket4");
            DropForeignKey("dbo.SocketSpentBucketItem3", "BUCKET_ID", "dbo.SocketSpentBucket3");
            DropForeignKey("dbo.SocketSpentBucketItem2", "BUCKET_ID", "dbo.SocketSpentBucket2");
            DropForeignKey("dbo.SocketSpentBucketItem10", "BUCKET_ID", "dbo.SocketSpentBucket10");
            DropIndex("dbo.SocketSpentBucketItem9", new[] { "BUCKET_ID" });
            DropIndex("dbo.SocketSpentBucketItem8", new[] { "BUCKET_ID" });
            DropIndex("dbo.SocketSpentBucketItem7", new[] { "BUCKET_ID" });
            DropIndex("dbo.SocketSpentBucketItem6", new[] { "BUCKET_ID" });
            DropIndex("dbo.SocketSpentBucketItem5", new[] { "BUCKET_ID" });
            DropIndex("dbo.SocketSpentBucketItem4", new[] { "BUCKET_ID" });
            DropIndex("dbo.SocketSpentBucketItem3", new[] { "BUCKET_ID" });
            DropIndex("dbo.SocketSpentBucketItem2", new[] { "BUCKET_ID" });
            DropIndex("dbo.SocketSpentBucketItem10", new[] { "BUCKET_ID" });
            DropTable("dbo.SocketSpentBucketItem9");
            DropTable("dbo.SocketSpentBucketItem8");
            DropTable("dbo.SocketSpentBucketItem7");
            DropTable("dbo.SocketSpentBucketItem6");
            DropTable("dbo.SocketSpentBucketItem5");
            DropTable("dbo.SocketSpentBucketItem4");
            DropTable("dbo.SocketSpentBucketItem3");
            DropTable("dbo.SocketSpentBucketItem2");
            DropTable("dbo.SocketSpentBucketItem10");
            DropTable("dbo.SocketSpentBucket9");
            DropTable("dbo.SocketSpentBucket8");
            DropTable("dbo.SocketSpentBucket7");
            DropTable("dbo.SocketSpentBucket6");
            DropTable("dbo.SocketSpentBucket5");
            DropTable("dbo.SocketSpentBucket4");
            DropTable("dbo.SocketSpentBucket3");
            DropTable("dbo.SocketSpentBucket2");
            DropTable("dbo.SocketSpentBucket10");
        }
    }
}
