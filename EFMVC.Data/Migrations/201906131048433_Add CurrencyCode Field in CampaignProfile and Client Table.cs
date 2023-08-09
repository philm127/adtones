namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCurrencyCodeFieldinCampaignProfileandClientTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CampaignProfile", "CurrencyCode", c => c.String());
            AddColumn("dbo.Client", "CurrencyCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Client", "CurrencyCode");
            DropColumn("dbo.CampaignProfile", "CurrencyCode");
        }
    }
}
