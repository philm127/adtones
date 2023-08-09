namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCurrencyRatetable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CurrencyRates",
                c => new
                    {
                        CurrencyRateId = c.Int(nullable: false, identity: true),
                        CurrencyCode = c.String(maxLength: 10),
                        CurrencyRateAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CurrencyRateId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CurrencyRates");
        }
    }
}
