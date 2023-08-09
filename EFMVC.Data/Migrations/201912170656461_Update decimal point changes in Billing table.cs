namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedecimalpointchangesinBillingtable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Billing", "FundAmount", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.Billing", "TaxPercantage", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.Billing", "TotalAmount", c => c.Decimal(nullable: false, precision: 18, scale: 6));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Billing", "TotalAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Billing", "TaxPercantage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Billing", "FundAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
