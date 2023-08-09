namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdtoneServerAdvertCategoryIdFieldinAdvertCategoryTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdvertCategories", "AdtoneServerAdvertCategoryId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdvertCategories", "AdtoneServerAdvertCategoryId");
        }
    }
}
