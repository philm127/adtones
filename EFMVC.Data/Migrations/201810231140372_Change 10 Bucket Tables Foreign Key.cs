namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change10BucketTablesForeignKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bucket10", "BUCKET_BATCH_ID", "dbo.BucketBatch10");
            DropForeignKey("dbo.Bucket2", "BUCKET_BATCH_ID", "dbo.BucketBatch2");
            DropForeignKey("dbo.Bucket3", "BUCKET_BATCH_ID", "dbo.BucketBatch3");
            DropForeignKey("dbo.Bucket4", "BUCKET_BATCH_ID", "dbo.BucketBatch4");
            DropForeignKey("dbo.Bucket5", "BUCKET_BATCH_ID", "dbo.BucketBatch5");
            DropForeignKey("dbo.Bucket6", "BUCKET_BATCH_ID", "dbo.BucketBatch6");
            DropForeignKey("dbo.Bucket7", "BUCKET_BATCH_ID", "dbo.BucketBatch7");
            DropForeignKey("dbo.Bucket8", "BUCKET_BATCH_ID", "dbo.BucketBatch8");
            DropForeignKey("dbo.Bucket9", "BUCKET_BATCH_ID", "dbo.BucketBatch9");
            AddForeignKey("dbo.Bucket10", "BUCKET_BATCH_ID", "dbo.BucketBatches", "Id");
            AddForeignKey("dbo.Bucket2", "BUCKET_BATCH_ID", "dbo.BucketBatches", "Id");
            AddForeignKey("dbo.Bucket3", "BUCKET_BATCH_ID", "dbo.BucketBatches", "Id");
            AddForeignKey("dbo.Bucket4", "BUCKET_BATCH_ID", "dbo.BucketBatches", "Id");
            AddForeignKey("dbo.Bucket5", "BUCKET_BATCH_ID", "dbo.BucketBatches", "Id");
            AddForeignKey("dbo.Bucket6", "BUCKET_BATCH_ID", "dbo.BucketBatches", "Id");
            AddForeignKey("dbo.Bucket7", "BUCKET_BATCH_ID", "dbo.BucketBatches", "Id");
            AddForeignKey("dbo.Bucket8", "BUCKET_BATCH_ID", "dbo.BucketBatches", "Id");
            AddForeignKey("dbo.Bucket9", "BUCKET_BATCH_ID", "dbo.BucketBatches", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bucket9", "BUCKET_BATCH_ID", "dbo.BucketBatches");
            DropForeignKey("dbo.Bucket8", "BUCKET_BATCH_ID", "dbo.BucketBatches");
            DropForeignKey("dbo.Bucket7", "BUCKET_BATCH_ID", "dbo.BucketBatches");
            DropForeignKey("dbo.Bucket6", "BUCKET_BATCH_ID", "dbo.BucketBatches");
            DropForeignKey("dbo.Bucket5", "BUCKET_BATCH_ID", "dbo.BucketBatches");
            DropForeignKey("dbo.Bucket4", "BUCKET_BATCH_ID", "dbo.BucketBatches");
            DropForeignKey("dbo.Bucket3", "BUCKET_BATCH_ID", "dbo.BucketBatches");
            DropForeignKey("dbo.Bucket2", "BUCKET_BATCH_ID", "dbo.BucketBatches");
            DropForeignKey("dbo.Bucket10", "BUCKET_BATCH_ID", "dbo.BucketBatches");
            AddForeignKey("dbo.Bucket9", "BUCKET_BATCH_ID", "dbo.BucketBatch9", "Id");
            AddForeignKey("dbo.Bucket8", "BUCKET_BATCH_ID", "dbo.BucketBatch8", "Id");
            AddForeignKey("dbo.Bucket7", "BUCKET_BATCH_ID", "dbo.BucketBatch7", "Id");
            AddForeignKey("dbo.Bucket6", "BUCKET_BATCH_ID", "dbo.BucketBatch6", "Id");
            AddForeignKey("dbo.Bucket5", "BUCKET_BATCH_ID", "dbo.BucketBatch5", "Id");
            AddForeignKey("dbo.Bucket4", "BUCKET_BATCH_ID", "dbo.BucketBatch4", "Id");
            AddForeignKey("dbo.Bucket3", "BUCKET_BATCH_ID", "dbo.BucketBatch3", "Id");
            AddForeignKey("dbo.Bucket2", "BUCKET_BATCH_ID", "dbo.BucketBatch2", "Id");
            AddForeignKey("dbo.Bucket10", "BUCKET_BATCH_ID", "dbo.BucketBatch10", "Id");
        }
    }
}
