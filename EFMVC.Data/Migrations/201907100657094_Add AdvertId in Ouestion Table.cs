namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdvertIdinOuestionTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Question", "AdvertId", c => c.Int());
            CreateIndex("dbo.Question", "AdvertId");
            AddForeignKey("dbo.Question", "AdvertId", "dbo.Advert", "AdvertId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Question", "AdvertId", "dbo.Advert");
            DropIndex("dbo.Question", new[] { "AdvertId" });
            DropColumn("dbo.Question", "AdvertId");
        }
    }
}
