namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNumOfNewUserinImportFileTrackTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImportFileTracks", "NumOfNewUser", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImportFileTracks", "NumOfNewUser");
        }
    }
}
