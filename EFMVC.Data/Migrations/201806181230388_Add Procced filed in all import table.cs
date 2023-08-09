namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProccedfiledinallimporttable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Import10", "Proceed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Import10", "AddedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Import2", "Proceed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Import2", "AddedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Import3", "Proceed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Import3", "AddedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Import4", "Proceed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Import4", "AddedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Import5", "Proceed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Import5", "AddedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Import6", "Proceed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Import6", "AddedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Import7", "Proceed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Import7", "AddedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Import8", "Proceed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Import8", "AddedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Import9", "Proceed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Import9", "AddedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Import9", "AddedDate");
            DropColumn("dbo.Import9", "Proceed");
            DropColumn("dbo.Import8", "AddedDate");
            DropColumn("dbo.Import8", "Proceed");
            DropColumn("dbo.Import7", "AddedDate");
            DropColumn("dbo.Import7", "Proceed");
            DropColumn("dbo.Import6", "AddedDate");
            DropColumn("dbo.Import6", "Proceed");
            DropColumn("dbo.Import5", "AddedDate");
            DropColumn("dbo.Import5", "Proceed");
            DropColumn("dbo.Import4", "AddedDate");
            DropColumn("dbo.Import4", "Proceed");
            DropColumn("dbo.Import3", "AddedDate");
            DropColumn("dbo.Import3", "Proceed");
            DropColumn("dbo.Import2", "AddedDate");
            DropColumn("dbo.Import2", "Proceed");
            DropColumn("dbo.Import10", "AddedDate");
            DropColumn("dbo.Import10", "Proceed");
        }
    }
}
