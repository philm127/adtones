namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProviderfieldinUserTokenLinkTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserTokenLinks", "Provider", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserTokenLinks", "Provider");
        }
    }
}
