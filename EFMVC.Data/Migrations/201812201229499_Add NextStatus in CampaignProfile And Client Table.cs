namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNextStatusinCampaignProfileAndClientTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CampaignProfile", "NextStatus", c => c.Boolean(nullable: false));
            AddColumn("dbo.Client", "NextStatus", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Client", "NextStatus");
            DropColumn("dbo.CampaignProfile", "NextStatus");
        }
    }
}
