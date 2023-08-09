namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCurrencyCodeFieldinBillingTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Billing", "CurrencyCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Billing", "CurrencyCode");
        }
    }
}
