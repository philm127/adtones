namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSpentBucketItemTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SpentBucketItem10",
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
                .ForeignKey("dbo.SpentBucket10", t => t.BUCKET_ID)
                .Index(t => t.BUCKET_ID);
            
            CreateTable(
                "dbo.SpentBucketItem2",
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
                .ForeignKey("dbo.SpentBucket2", t => t.BUCKET_ID)
                .Index(t => t.BUCKET_ID);
            
            CreateTable(
                "dbo.SpentBucketItem3",
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
                .ForeignKey("dbo.SpentBucket3", t => t.BUCKET_ID)
                .Index(t => t.BUCKET_ID);
            
            CreateTable(
                "dbo.SpentBucketItem4",
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
                .ForeignKey("dbo.SpentBucket4", t => t.BUCKET_ID)
                .Index(t => t.BUCKET_ID);
            
            CreateTable(
                "dbo.SpentBucketItem5",
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
                .ForeignKey("dbo.SpentBucket5", t => t.BUCKET_ID)
                .Index(t => t.BUCKET_ID);
            
            CreateTable(
                "dbo.SpentBucketItem6",
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
                .ForeignKey("dbo.SpentBucket6", t => t.BUCKET_ID)
                .Index(t => t.BUCKET_ID);
            
            CreateTable(
                "dbo.SpentBucketItem7",
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
                .ForeignKey("dbo.SpentBucket7", t => t.BUCKET_ID)
                .Index(t => t.BUCKET_ID);
            
            CreateTable(
                "dbo.SpentBucketItem8",
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
                .ForeignKey("dbo.SpentBucket8", t => t.BUCKET_ID)
                .Index(t => t.BUCKET_ID);
            
            CreateTable(
                "dbo.SpentBucketItem9",
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
                .ForeignKey("dbo.SpentBucket9", t => t.BUCKET_ID)
                .Index(t => t.BUCKET_ID);
            
            CreateTable(
                "dbo.SpentBucketItems",
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
                .ForeignKey("dbo.SpentBuckets", t => t.BUCKET_ID)
                .Index(t => t.BUCKET_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SpentBucketItems", "BUCKET_ID", "dbo.SpentBuckets");
            DropForeignKey("dbo.SpentBucketItem9", "BUCKET_ID", "dbo.SpentBucket9");
            DropForeignKey("dbo.SpentBucketItem8", "BUCKET_ID", "dbo.SpentBucket8");
            DropForeignKey("dbo.SpentBucketItem7", "BUCKET_ID", "dbo.SpentBucket7");
            DropForeignKey("dbo.SpentBucketItem6", "BUCKET_ID", "dbo.SpentBucket6");
            DropForeignKey("dbo.SpentBucketItem5", "BUCKET_ID", "dbo.SpentBucket5");
            DropForeignKey("dbo.SpentBucketItem4", "BUCKET_ID", "dbo.SpentBucket4");
            DropForeignKey("dbo.SpentBucketItem3", "BUCKET_ID", "dbo.SpentBucket3");
            DropForeignKey("dbo.SpentBucketItem2", "BUCKET_ID", "dbo.SpentBucket2");
            DropForeignKey("dbo.SpentBucketItem10", "BUCKET_ID", "dbo.SpentBucket10");
            DropIndex("dbo.SpentBucketItems", new[] { "BUCKET_ID" });
            DropIndex("dbo.SpentBucketItem9", new[] { "BUCKET_ID" });
            DropIndex("dbo.SpentBucketItem8", new[] { "BUCKET_ID" });
            DropIndex("dbo.SpentBucketItem7", new[] { "BUCKET_ID" });
            DropIndex("dbo.SpentBucketItem6", new[] { "BUCKET_ID" });
            DropIndex("dbo.SpentBucketItem5", new[] { "BUCKET_ID" });
            DropIndex("dbo.SpentBucketItem4", new[] { "BUCKET_ID" });
            DropIndex("dbo.SpentBucketItem3", new[] { "BUCKET_ID" });
            DropIndex("dbo.SpentBucketItem2", new[] { "BUCKET_ID" });
            DropIndex("dbo.SpentBucketItem10", new[] { "BUCKET_ID" });
            DropTable("dbo.SpentBucketItems");
            DropTable("dbo.SpentBucketItem9");
            DropTable("dbo.SpentBucketItem8");
            DropTable("dbo.SpentBucketItem7");
            DropTable("dbo.SpentBucketItem6");
            DropTable("dbo.SpentBucketItem5");
            DropTable("dbo.SpentBucketItem4");
            DropTable("dbo.SpentBucketItem3");
            DropTable("dbo.SpentBucketItem2");
            DropTable("dbo.SpentBucketItem10");
        }
    }
}
