namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveBucketBatchRelationFromAllBucketTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Buckets", "BUCKET_BATCH_ID", "dbo.BucketBatches");
            DropForeignKey("dbo.Bucket10", "BUCKET_BATCH_ID", "dbo.BucketBatches");
            DropForeignKey("dbo.Bucket2", "BUCKET_BATCH_ID", "dbo.BucketBatches");
            DropForeignKey("dbo.Bucket3", "BUCKET_BATCH_ID", "dbo.BucketBatches");
            DropForeignKey("dbo.Bucket4", "BUCKET_BATCH_ID", "dbo.BucketBatches");
            DropForeignKey("dbo.Bucket5", "BUCKET_BATCH_ID", "dbo.BucketBatches");
            DropForeignKey("dbo.Bucket6", "BUCKET_BATCH_ID", "dbo.BucketBatches");
            DropForeignKey("dbo.Bucket7", "BUCKET_BATCH_ID", "dbo.BucketBatches");
            DropForeignKey("dbo.Bucket8", "BUCKET_BATCH_ID", "dbo.BucketBatches");
            DropForeignKey("dbo.Bucket9", "BUCKET_BATCH_ID", "dbo.BucketBatches");
            DropIndex("dbo.Buckets", new[] { "BUCKET_BATCH_ID" });
            DropIndex("dbo.Bucket10", new[] { "BUCKET_BATCH_ID" });
            DropIndex("dbo.Bucket2", new[] { "BUCKET_BATCH_ID" });
            DropIndex("dbo.Bucket3", new[] { "BUCKET_BATCH_ID" });
            DropIndex("dbo.Bucket4", new[] { "BUCKET_BATCH_ID" });
            DropIndex("dbo.Bucket5", new[] { "BUCKET_BATCH_ID" });
            DropIndex("dbo.Bucket6", new[] { "BUCKET_BATCH_ID" });
            DropIndex("dbo.Bucket7", new[] { "BUCKET_BATCH_ID" });
            DropIndex("dbo.Bucket8", new[] { "BUCKET_BATCH_ID" });
            DropIndex("dbo.Bucket9", new[] { "BUCKET_BATCH_ID" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Bucket9", "BUCKET_BATCH_ID");
            CreateIndex("dbo.Bucket8", "BUCKET_BATCH_ID");
            CreateIndex("dbo.Bucket7", "BUCKET_BATCH_ID");
            CreateIndex("dbo.Bucket6", "BUCKET_BATCH_ID");
            CreateIndex("dbo.Bucket5", "BUCKET_BATCH_ID");
            CreateIndex("dbo.Bucket4", "BUCKET_BATCH_ID");
            CreateIndex("dbo.Bucket3", "BUCKET_BATCH_ID");
            CreateIndex("dbo.Bucket2", "BUCKET_BATCH_ID");
            CreateIndex("dbo.Bucket10", "BUCKET_BATCH_ID");
            CreateIndex("dbo.Buckets", "BUCKET_BATCH_ID");
            AddForeignKey("dbo.Bucket9", "BUCKET_BATCH_ID", "dbo.BucketBatches", "Id");
            AddForeignKey("dbo.Bucket8", "BUCKET_BATCH_ID", "dbo.BucketBatches", "Id");
            AddForeignKey("dbo.Bucket7", "BUCKET_BATCH_ID", "dbo.BucketBatches", "Id");
            AddForeignKey("dbo.Bucket6", "BUCKET_BATCH_ID", "dbo.BucketBatches", "Id");
            AddForeignKey("dbo.Bucket5", "BUCKET_BATCH_ID", "dbo.BucketBatches", "Id");
            AddForeignKey("dbo.Bucket4", "BUCKET_BATCH_ID", "dbo.BucketBatches", "Id");
            AddForeignKey("dbo.Bucket3", "BUCKET_BATCH_ID", "dbo.BucketBatches", "Id");
            AddForeignKey("dbo.Bucket2", "BUCKET_BATCH_ID", "dbo.BucketBatches", "Id");
            AddForeignKey("dbo.Bucket10", "BUCKET_BATCH_ID", "dbo.BucketBatches", "Id");
            AddForeignKey("dbo.Buckets", "BUCKET_BATCH_ID", "dbo.BucketBatches", "Id");
        }
    }
}
