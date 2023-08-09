namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add10duplicateBucketBatchTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BucketBatch10",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BATCH_STATUS_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BucketBatch2",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BATCH_STATUS_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BucketBatch3",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BATCH_STATUS_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BucketBatch4",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BATCH_STATUS_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BucketBatch5",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BATCH_STATUS_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BucketBatch6",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BATCH_STATUS_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BucketBatch7",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BATCH_STATUS_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BucketBatch8",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BATCH_STATUS_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BucketBatch9",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BATCH_STATUS_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BucketBatch9");
            DropTable("dbo.BucketBatch8");
            DropTable("dbo.BucketBatch7");
            DropTable("dbo.BucketBatch6");
            DropTable("dbo.BucketBatch5");
            DropTable("dbo.BucketBatch4");
            DropTable("dbo.BucketBatch3");
            DropTable("dbo.BucketBatch2");
            DropTable("dbo.BucketBatch10");
        }
    }
}
