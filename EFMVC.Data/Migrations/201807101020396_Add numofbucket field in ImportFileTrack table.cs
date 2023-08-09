namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddnumofbucketfieldinImportFileTracktable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImportFileTracks", "NumOfBucket", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImportFileTracks", "NumOfBucket");
        }
    }
}
