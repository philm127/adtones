namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAvailableBudgetfieldinCampaignBudgettable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CampaignBudgets", "AvailableBudget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CampaignBudgets", "AvailableBudget");
        }
    }
}
