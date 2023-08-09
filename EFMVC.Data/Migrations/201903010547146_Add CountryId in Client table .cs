namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCountryIdinClienttable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Client", "CountryId", c => c.Int());
            AddColumn("dbo.ProfileMatchInformations", "ProfileType", c => c.String());
            CreateIndex("dbo.Client", "CountryId");
            AddForeignKey("dbo.Client", "CountryId", "dbo.Country", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Client", "CountryId", "dbo.Country");
            DropIndex("dbo.Client", new[] { "CountryId" });
            DropColumn("dbo.ProfileMatchInformations", "ProfileType");
            DropColumn("dbo.Client", "CountryId");
        }
    }
}
