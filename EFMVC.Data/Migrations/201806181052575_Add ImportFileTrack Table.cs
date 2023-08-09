namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImportFileTrackTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImportFileTracks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumOfTextFile = c.Int(nullable: false),
                        NumOfTextLine = c.Int(nullable: false),
                        AddedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Imports", "Proceed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Imports", "AddedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Imports", "AddedDate");
            DropColumn("dbo.Imports", "Proceed");
            DropTable("dbo.ImportFileTracks");
        }
    }
}
