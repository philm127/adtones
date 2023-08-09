namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCountryFieldinOperatortable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operators", "CountryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Operators", "CountryId");
            AddForeignKey("dbo.Operators", "CountryId", "dbo.Country", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Operators", "CountryId", "dbo.Country");
            DropIndex("dbo.Operators", new[] { "CountryId" });
            DropColumn("dbo.Operators", "CountryId");
        }
    }
}
