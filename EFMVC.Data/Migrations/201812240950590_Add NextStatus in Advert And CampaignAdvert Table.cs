namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNextStatusinAdvertAndCampaignAdvertTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Advert", "NextStatus", c => c.Boolean(nullable: false));
            AddColumn("dbo.CampaignAdverts", "NextStatus", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CampaignAdverts", "NextStatus");
            DropColumn("dbo.Advert", "NextStatus");
        }
    }
}
