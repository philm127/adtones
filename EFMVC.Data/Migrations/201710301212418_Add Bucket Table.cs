namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBucketTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Buckets",
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
                .ForeignKey("dbo.BucketBatches", t => t.BUCKET_BATCH_ID)
                .Index(t => t.BUCKET_BATCH_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Buckets", "BUCKET_BATCH_ID", "dbo.BucketBatches");
            DropIndex("dbo.Buckets", new[] { "BUCKET_BATCH_ID" });
            DropTable("dbo.Buckets");
        }
    }
}
