namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNumofUserfieldinImportFileTracktable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfileAdvertsReceiveds", "Proceed", c => c.Int(nullable: false));
            AddColumn("dbo.ImportFileTracks", "NumOfUser", c => c.Int(nullable: false));
            AddColumn("dbo.ImportFileTracks", "NumOfRemovedUser", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImportFileTracks", "NumOfRemovedUser");
            DropColumn("dbo.ImportFileTracks", "NumOfUser");
            DropColumn("dbo.UserProfileAdvertsReceiveds", "Proceed");
        }
    }
}
