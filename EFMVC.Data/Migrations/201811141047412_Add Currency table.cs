namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCurrencytable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Currencies",
                c => new
                    {
                        CurrencyId = c.Int(nullable: false, identity: true),
                        CurrencyCode = c.String(maxLength: 10),
                        CountryId = c.Int(),
                    })
                .PrimaryKey(t => t.CurrencyId)
                .ForeignKey("dbo.Country", t => t.CountryId)
                .Index(t => t.CountryId);
            
            AddColumn("dbo.Contacts", "CurrencyId", c => c.Int());
            CreateIndex("dbo.Contacts", "CurrencyId");
            AddForeignKey("dbo.Contacts", "CurrencyId", "dbo.Currencies", "CurrencyId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contacts", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.Currencies", "CountryId", "dbo.Country");
            DropIndex("dbo.Currencies", new[] { "CountryId" });
            DropIndex("dbo.Contacts", new[] { "CurrencyId" });
            DropColumn("dbo.Contacts", "CurrencyId");
            DropTable("dbo.Currencies");
        }
    }
}
