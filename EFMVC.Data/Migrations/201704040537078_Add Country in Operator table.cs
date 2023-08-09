namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCountryinOperatortable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operators", "CountryId", c => c.Int());
            CreateIndex("dbo.Operators", "CountryId");
            AddForeignKey("dbo.Operators", "CountryId", "dbo.Country", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Operators", "CountryId", "dbo.Country");
            DropIndex("dbo.Operators", new[] { "CountryId" });
            DropColumn("dbo.Operators", "CountryId");
        }
    }
}
