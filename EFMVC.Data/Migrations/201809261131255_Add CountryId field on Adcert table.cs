namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCountryIdfieldonAdcerttable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Advert", "CountryId", c => c.Int());
            CreateIndex("dbo.Advert", "CountryId");
            AddForeignKey("dbo.Advert", "CountryId", "dbo.Country", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Advert", "CountryId", "dbo.Country");
            DropIndex("dbo.Advert", new[] { "CountryId" });
            DropColumn("dbo.Advert", "CountryId");
        }
    }
}
