namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdtoneServeCountryIdInCountryTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Country", "AdtoneServeCountryId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Country", "AdtoneServeCountryId");
        }
    }
}
