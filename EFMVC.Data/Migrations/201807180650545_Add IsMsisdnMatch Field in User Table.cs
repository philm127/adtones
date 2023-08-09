namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsMsisdnMatchFieldinUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "IsMsisdnMatch", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "IsMsisdnMatch");
        }
    }
}
