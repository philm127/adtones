namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConnectionStringInCampaignProfiletable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CampaignProfile", "ConnectionString", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CampaignProfile", "ConnectionString");
        }
    }
}
