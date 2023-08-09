namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmailCostAndSMSCostinOperatorTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operators", "EmailCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Operators", "SmsCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Operators", "CurrencyId", c => c.Int());
            CreateIndex("dbo.Operators", "CurrencyId");
            AddForeignKey("dbo.Operators", "CurrencyId", "dbo.Currencies", "CurrencyId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Operators", "CurrencyId", "dbo.Currencies");
            DropIndex("dbo.Operators", new[] { "CurrencyId" });
            DropColumn("dbo.Operators", "CurrencyId");
            DropColumn("dbo.Operators", "SmsCost");
            DropColumn("dbo.Operators", "EmailCost");
        }
    }
}
