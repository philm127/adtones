namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdvertCategoryIdforeignkeytoadverttable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Advert", "AdvertCategoryId", c => c.Int());
            CreateIndex("dbo.Advert", "AdvertCategoryId");
            AddForeignKey("dbo.Advert", "AdvertCategoryId", "dbo.AdvertCategories", "AdvertCategoryId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Advert", "AdvertCategoryId", "dbo.AdvertCategories");
            DropIndex("dbo.Advert", new[] { "AdvertCategoryId" });
            DropColumn("dbo.Advert", "AdvertCategoryId");
        }
    }
}
