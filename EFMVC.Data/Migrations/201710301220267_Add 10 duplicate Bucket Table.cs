namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add10duplicateBucketTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bucket10",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_BATCH_ID = c.Int(),
                        MSISDN = c.String(maxLength: 50),
                        ITEM_TOTAL = c.Int(),
                        EMAIL_ADDRESS = c.String(maxLength: 255),
                        MsUserProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BucketBatch10", t => t.BUCKET_BATCH_ID)
                .Index(t => t.BUCKET_BATCH_ID);
            
            CreateTable(
                "dbo.Bucket2",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_BATCH_ID = c.Int(),
                        MSISDN = c.String(maxLength: 50),
                        ITEM_TOTAL = c.Int(),
                        EMAIL_ADDRESS = c.String(maxLength: 255),
                        MsUserProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BucketBatch2", t => t.BUCKET_BATCH_ID)
                .Index(t => t.BUCKET_BATCH_ID);
            
            CreateTable(
                "dbo.Bucket3",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_BATCH_ID = c.Int(),
                        MSISDN = c.String(maxLength: 50),
                        ITEM_TOTAL = c.Int(),
                        EMAIL_ADDRESS = c.String(maxLength: 255),
                        MsUserProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BucketBatch3", t => t.BUCKET_BATCH_ID)
                .Index(t => t.BUCKET_BATCH_ID);
            
            CreateTable(
                "dbo.Bucket4",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_BATCH_ID = c.Int(),
                        MSISDN = c.String(maxLength: 50),
                        ITEM_TOTAL = c.Int(),
                        EMAIL_ADDRESS = c.String(maxLength: 255),
                        MsUserProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BucketBatch4", t => t.BUCKET_BATCH_ID)
                .Index(t => t.BUCKET_BATCH_ID);
            
            CreateTable(
                "dbo.Bucket5",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_BATCH_ID = c.Int(),
                        MSISDN = c.String(maxLength: 50),
                        ITEM_TOTAL = c.Int(),
                        EMAIL_ADDRESS = c.String(maxLength: 255),
                        MsUserProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BucketBatch5", t => t.BUCKET_BATCH_ID)
                .Index(t => t.BUCKET_BATCH_ID);
            
            CreateTable(
                "dbo.Bucket6",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_BATCH_ID = c.Int(),
                        MSISDN = c.String(maxLength: 50),
                        ITEM_TOTAL = c.Int(),
                        EMAIL_ADDRESS = c.String(maxLength: 255),
                        MsUserProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BucketBatch6", t => t.BUCKET_BATCH_ID)
                .Index(t => t.BUCKET_BATCH_ID);
            
            CreateTable(
                "dbo.Bucket7",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_BATCH_ID = c.Int(),
                        MSISDN = c.String(maxLength: 50),
                        ITEM_TOTAL = c.Int(),
                        EMAIL_ADDRESS = c.String(maxLength: 255),
                        MsUserProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BucketBatch7", t => t.BUCKET_BATCH_ID)
                .Index(t => t.BUCKET_BATCH_ID);
            
            CreateTable(
                "dbo.Bucket8",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_BATCH_ID = c.Int(),
                        MSISDN = c.String(maxLength: 50),
                        ITEM_TOTAL = c.Int(),
                        EMAIL_ADDRESS = c.String(maxLength: 255),
                        MsUserProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BucketBatch8", t => t.BUCKET_BATCH_ID)
                .Index(t => t.BUCKET_BATCH_ID);
            
            CreateTable(
                "dbo.Bucket9",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BUCKET_BATCH_ID = c.Int(),
                        MSISDN = c.String(maxLength: 50),
                        ITEM_TOTAL = c.Int(),
                        EMAIL_ADDRESS = c.String(maxLength: 255),
                        MsUserProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BucketBatch9", t => t.BUCKET_BATCH_ID)
                .Index(t => t.BUCKET_BATCH_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bucket9", "BUCKET_BATCH_ID", "dbo.BucketBatch9");
            DropForeignKey("dbo.Bucket8", "BUCKET_BATCH_ID", "dbo.BucketBatch8");
            DropForeignKey("dbo.Bucket7", "BUCKET_BATCH_ID", "dbo.BucketBatch7");
            DropForeignKey("dbo.Bucket6", "BUCKET_BATCH_ID", "dbo.BucketBatch6");
            DropForeignKey("dbo.Bucket5", "BUCKET_BATCH_ID", "dbo.BucketBatch5");
            DropForeignKey("dbo.Bucket4", "BUCKET_BATCH_ID", "dbo.BucketBatch4");
            DropForeignKey("dbo.Bucket3", "BUCKET_BATCH_ID", "dbo.BucketBatch3");
            DropForeignKey("dbo.Bucket2", "BUCKET_BATCH_ID", "dbo.BucketBatch2");
            DropForeignKey("dbo.Bucket10", "BUCKET_BATCH_ID", "dbo.BucketBatch10");
            DropIndex("dbo.Bucket9", new[] { "BUCKET_BATCH_ID" });
            DropIndex("dbo.Bucket8", new[] { "BUCKET_BATCH_ID" });
            DropIndex("dbo.Bucket7", new[] { "BUCKET_BATCH_ID" });
            DropIndex("dbo.Bucket6", new[] { "BUCKET_BATCH_ID" });
            DropIndex("dbo.Bucket5", new[] { "BUCKET_BATCH_ID" });
            DropIndex("dbo.Bucket4", new[] { "BUCKET_BATCH_ID" });
            DropIndex("dbo.Bucket3", new[] { "BUCKET_BATCH_ID" });
            DropIndex("dbo.Bucket2", new[] { "BUCKET_BATCH_ID" });
            DropIndex("dbo.Bucket10", new[] { "BUCKET_BATCH_ID" });
            DropTable("dbo.Bucket9");
            DropTable("dbo.Bucket8");
            DropTable("dbo.Bucket7");
            DropTable("dbo.Bucket6");
            DropTable("dbo.Bucket5");
            DropTable("dbo.Bucket4");
            DropTable("dbo.Bucket3");
            DropTable("dbo.Bucket2");
            DropTable("dbo.Bucket10");
        }
    }
}
