namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNumofplayfieldinImportFileTracktable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImportFileTracks", "NumOfPlay", c => c.Int(nullable: false));
            AddColumn("dbo.ImportFileTracks", "NumOfSMS", c => c.Int(nullable: false));
            AddColumn("dbo.ImportFileTracks", "NumOfEmail", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImportFileTracks", "NumOfEmail");
            DropColumn("dbo.ImportFileTracks", "NumOfSMS");
            DropColumn("dbo.ImportFileTracks", "NumOfPlay");
        }
    }
}
