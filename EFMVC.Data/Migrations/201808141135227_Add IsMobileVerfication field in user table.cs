namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsMobileVerficationfieldinusertable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "IsMobileVerfication", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "IsMobileVerfication");
        }
    }
}
