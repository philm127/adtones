namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProfileMatchInfoTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProfileMatchInformations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProfileName = c.String(maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CountryId = c.Int(),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Country", t => t.CountryId)
                .Index(t => t.CountryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProfileMatchInformations", "CountryId", "dbo.Country");
            DropIndex("dbo.ProfileMatchInformations", new[] { "CountryId" });
            DropTable("dbo.ProfileMatchInformations");
        }
    }
}
