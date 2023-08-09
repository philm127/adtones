namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCampProfileIdinAdvertTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Advert", "CampProfileId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Advert", "CampProfileId");
        }
    }
}
