namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTIBCOResponseCodeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TIBCOResponseCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReturnCode = c.String(maxLength: 15),
                        Description = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TIBCOResponseCodes");
        }
    }
}
