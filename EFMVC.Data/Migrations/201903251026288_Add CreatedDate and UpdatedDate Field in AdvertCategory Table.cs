namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreatedDateandUpdatedDateFieldinAdvertCategoryTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdvertCategories", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.AdvertCategories", "UpdatedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdvertCategories", "UpdatedDate");
            DropColumn("dbo.AdvertCategories", "CreatedDate");
        }
    }
}
