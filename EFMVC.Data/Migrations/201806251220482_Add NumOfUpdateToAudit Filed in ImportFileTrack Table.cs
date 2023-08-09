namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNumOfUpdateToAuditFiledinImportFileTrackTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImportFileTracks", "NumOfUpdateToAudit", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImportFileTracks", "NumOfUpdateToAudit");
        }
    }
}
