namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUpdatedByinadverttable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Advert", "UpdatedBy", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Advert", "UpdatedBy");
        }
    }
}
