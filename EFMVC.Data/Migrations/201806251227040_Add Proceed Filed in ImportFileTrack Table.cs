namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProceedFiledinImportFileTrackTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImportFileTracks", "Proceed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImportFileTracks", "Proceed");
        }
    }
}
