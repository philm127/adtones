namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCountryIdinContacttable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "CountryId", c => c.Int());
            CreateIndex("dbo.Contacts", "CountryId");
            AddForeignKey("dbo.Contacts", "CountryId", "dbo.Country", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contacts", "CountryId", "dbo.Country");
            DropIndex("dbo.Contacts", new[] { "CountryId" });
            DropColumn("dbo.Contacts", "CountryId");
        }
    }
}
