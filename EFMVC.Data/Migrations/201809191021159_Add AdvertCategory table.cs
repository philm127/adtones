namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdvertCategorytable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdvertCategories",
                c => new
                    {
                        AdvertCategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.AdvertCategoryId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AdvertCategories");
        }
    }
}
