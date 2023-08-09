namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSpentBucketTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.SocketSpentBucket10", newName: "SpentBucket10");
            DropForeignKey("dbo.SocketSpentBucketItems", "BUCKET_ID", "dbo.SocketSpentBuckets");
            DropForeignKey("dbo.SocketSpentBucketItem10", "BUCKET_ID", "dbo.SocketSpentBucket10");
            DropForeignKey("dbo.SocketSpentBucketItem2", "BUCKET_ID", "dbo.SocketSpentBucket2");
            DropForeignKey("dbo.SocketSpentBucketItem3", "BUCKET_ID", "dbo.SocketSpentBucket3");
            DropForeignKey("dbo.SocketSpentBucketItem4", "BUCKET_ID", "dbo.SocketSpentBucket4");
            DropForeignKey("dbo.SocketSpentBucketItem5", "BUCKET_ID", "dbo.SocketSpentBucket5");
            DropForeignKey("dbo.SocketSpentBucketItem6", "BUCKET_ID", "dbo.SocketSpentBucket6");
            DropForeignKey("dbo.SocketSpentBucketItem7", "BUCKET_ID", "dbo.SocketSpentBucket7");
            DropForeignKey("dbo.SocketSpentBucketItem8", "BUCKET_ID", "dbo.SocketSpentBucket8");
            DropForeignKey("dbo.SocketSpentBucketItem9", "BUCKET_ID", "dbo.SocketSpentBucket9");
            DropIndex("dbo.SocketSpentBucketItems", new[] { "BUCKET_ID" });
            DropIndex("dbo.SocketSpentBucketItem10", new[] { "BUCKET_ID" });
            DropIndex("dbo.SocketSpentBucketItem2", new[] { "BUCKET_ID" });
            DropIndex("dbo.SocketSpentBucketItem3", new[] { "BUCKET_ID" });
            DropIndex("dbo.SocketSpentBucketItem4", new[] { "BUCKET_ID" });
            DropIndex("dbo.SocketSpentBucketItem5", new[] { "BUCKET_ID" });
            DropIndex("dbo.SocketSpentBucketItem6", new[] { "BUCKET_ID" });
            DropIndex("dbo.SocketSpentBucketItem7", new[] { "BUCKET_ID" });
            DropIndex("dbo.SocketSpentBucketItem8", new[] { "BUCKET_ID" });
            DropIndex("dbo.SocketSpentBucketItem9", new[] { "BUCKET_ID" });
            CreateTable(
                "dbo.SpentBucket2",
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
                "dbo.SpentBucket3",
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
                "dbo.SpentBucket4",
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
                "dbo.SpentBucket5",
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
                "dbo.SpentBucket6",
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
                "dbo.SpentBucket7",
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
                "dbo.SpentBucket8",
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
                "dbo.SpentBucket9",
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
                "dbo.SpentBuckets",
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
            
            DropTable("dbo.SocketSpentBucket2");
            DropTable("dbo.SocketSpentBucket3");
            DropTable("dbo.SocketSpentBucket4");
            DropTable("dbo.SocketSpentBucket5");
            DropTable("dbo.SocketSpentBucket6");
            DropTable("dbo.SocketSpentBucket7");
            DropTable("dbo.SocketSpentBucket8");
            DropTable("dbo.SocketSpentBucket9");
            DropTable("dbo.SocketSpentBucketItems");
            DropTable("dbo.SocketSpentBuckets");
            DropTable("dbo.SocketSpentBucketItem10");
            DropTable("dbo.SocketSpentBucketItem2");
            DropTable("dbo.SocketSpentBucketItem3");
            DropTable("dbo.SocketSpentBucketItem4");
            DropTable("dbo.SocketSpentBucketItem5");
            DropTable("dbo.SocketSpentBucketItem6");
            DropTable("dbo.SocketSpentBucketItem7");
            DropTable("dbo.SocketSpentBucketItem8");
            DropTable("dbo.SocketSpentBucketItem9");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SocketSpentBuckets",
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
                "dbo.SocketSpentBucketItems",
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
            
            DropTable("dbo.SpentBuckets");
            DropTable("dbo.SpentBucket9");
            DropTable("dbo.SpentBucket8");
            DropTable("dbo.SpentBucket7");
            DropTable("dbo.SpentBucket6");
            DropTable("dbo.SpentBucket5");
            DropTable("dbo.SpentBucket4");
            DropTable("dbo.SpentBucket3");
            DropTable("dbo.SpentBucket2");
            CreateIndex("dbo.SocketSpentBucketItem9", "BUCKET_ID");
            CreateIndex("dbo.SocketSpentBucketItem8", "BUCKET_ID");
            CreateIndex("dbo.SocketSpentBucketItem7", "BUCKET_ID");
            CreateIndex("dbo.SocketSpentBucketItem6", "BUCKET_ID");
            CreateIndex("dbo.SocketSpentBucketItem5", "BUCKET_ID");
            CreateIndex("dbo.SocketSpentBucketItem4", "BUCKET_ID");
            CreateIndex("dbo.SocketSpentBucketItem3", "BUCKET_ID");
            CreateIndex("dbo.SocketSpentBucketItem2", "BUCKET_ID");
            CreateIndex("dbo.SocketSpentBucketItem10", "BUCKET_ID");
            CreateIndex("dbo.SocketSpentBucketItems", "BUCKET_ID");
            AddForeignKey("dbo.SocketSpentBucketItem9", "BUCKET_ID", "dbo.SocketSpentBucket9", "Id");
            AddForeignKey("dbo.SocketSpentBucketItem8", "BUCKET_ID", "dbo.SocketSpentBucket8", "Id");
            AddForeignKey("dbo.SocketSpentBucketItem7", "BUCKET_ID", "dbo.SocketSpentBucket7", "Id");
            AddForeignKey("dbo.SocketSpentBucketItem6", "BUCKET_ID", "dbo.SocketSpentBucket6", "Id");
            AddForeignKey("dbo.SocketSpentBucketItem5", "BUCKET_ID", "dbo.SocketSpentBucket5", "Id");
            AddForeignKey("dbo.SocketSpentBucketItem4", "BUCKET_ID", "dbo.SocketSpentBucket4", "Id");
            AddForeignKey("dbo.SocketSpentBucketItem3", "BUCKET_ID", "dbo.SocketSpentBucket3", "Id");
            AddForeignKey("dbo.SocketSpentBucketItem2", "BUCKET_ID", "dbo.SocketSpentBucket2", "Id");
            AddForeignKey("dbo.SocketSpentBucketItem10", "BUCKET_ID", "dbo.SocketSpentBucket10", "Id");
            AddForeignKey("dbo.SocketSpentBucketItems", "BUCKET_ID", "dbo.SocketSpentBuckets", "Id");
            RenameTable(name: "dbo.SpentBucket10", newName: "SocketSpentBucket10");
        }
    }
}
