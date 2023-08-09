namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBucketBatchStatusTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BucketBatchStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        STATUS = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BucketBatchStatus");
        }
    }
}
