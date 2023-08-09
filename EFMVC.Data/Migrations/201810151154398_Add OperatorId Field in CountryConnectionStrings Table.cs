namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOperatorIdFieldinCountryConnectionStringsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CountryConnectionStrings", "OperatorId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CountryConnectionStrings", "OperatorId");
        }
    }
}
