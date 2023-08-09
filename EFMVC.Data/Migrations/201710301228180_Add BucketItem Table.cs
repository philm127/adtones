namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBucketItemTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BucketItems",
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
                .ForeignKey("dbo.Buckets", t => t.BUCKET_ID)
                .Index(t => t.BUCKET_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BucketItems", "BUCKET_ID", "dbo.Buckets");
            DropIndex("dbo.BucketItems", new[] { "BUCKET_ID" });
            DropTable("dbo.BucketItems");
        }
    }
}
