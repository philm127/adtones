namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCountryIdFieldinAdvertCategoryTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdvertCategories", "CountryId", c => c.Int());
            CreateIndex("dbo.AdvertCategories", "CountryId");
            AddForeignKey("dbo.AdvertCategories", "CountryId", "dbo.Country", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AdvertCategories", "CountryId", "dbo.Country");
            DropIndex("dbo.AdvertCategories", new[] { "CountryId" });
            DropColumn("dbo.AdvertCategories", "CountryId");
        }
    }
}
