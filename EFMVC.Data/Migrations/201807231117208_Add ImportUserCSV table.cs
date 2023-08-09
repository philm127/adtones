namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImportUserCSVtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImportUserCSVs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PhoneNumber = c.String(maxLength: 30),
                        OperationType = c.String(maxLength: 1),
                        Email = c.String(maxLength: 100),
                        DateCreated = c.String(maxLength: 30),
                        AddedDate = c.DateTime(nullable: false),
                        Proceed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ImportUserCSVs");
        }
    }
}
