namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddoperatorfieldinImportfiletable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImportFileTracks", "OperatorId", c => c.Int());
            CreateIndex("dbo.ImportFileTracks", "OperatorId");
            AddForeignKey("dbo.ImportFileTracks", "OperatorId", "dbo.Operators", "OperatorId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ImportFileTracks", "OperatorId", "dbo.Operators");
            DropIndex("dbo.ImportFileTracks", new[] { "OperatorId" });
            DropColumn("dbo.ImportFileTracks", "OperatorId");
        }
    }
}
