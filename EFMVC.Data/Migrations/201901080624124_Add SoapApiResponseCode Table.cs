namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSoapApiResponseCodeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SoapApiResponseCodes",
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
            DropTable("dbo.SoapApiResponseCodes");
        }
    }
}
