namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCountryConnectionStringTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CountryConnectionStrings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountryId = c.Int(),
                        ConnectionString = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            //DropColumn("dbo.CampaignProfile", "ConnectionString");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CampaignProfile", "ConnectionString", c => c.String());
            DropTable("dbo.CountryConnectionStrings");
        }
    }
}
