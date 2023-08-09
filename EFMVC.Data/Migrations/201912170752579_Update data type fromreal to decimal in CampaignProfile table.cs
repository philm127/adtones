namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedatatypefromrealtodecimalinCampaignProfiletable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CampaignProfile", "TotalBudget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.CampaignProfile", "TotalCredit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.CampaignProfile", "AvailableCredit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CampaignProfile", "AvailableCredit", c => c.Single(nullable: false));
            AlterColumn("dbo.CampaignProfile", "TotalCredit", c => c.Single(nullable: false));
            AlterColumn("dbo.CampaignProfile", "TotalBudget", c => c.Single(nullable: false));
        }
    }
}
