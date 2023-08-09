namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCountryCodeInCountryTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Country", "CountryCode", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Country", "CountryCode");
        }
    }
}
