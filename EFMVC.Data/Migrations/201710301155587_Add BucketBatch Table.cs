namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBucketBatchTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BucketBatches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BATCH_STATUS_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BucketBatches");
        }
    }
}
