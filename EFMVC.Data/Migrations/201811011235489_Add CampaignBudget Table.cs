namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCampaignBudgetTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CampaignBudgets",
                c => new
                    {
                        CampaignBudgetId = c.Int(nullable: false, identity: true),
                        CampaignProfileId = c.Int(nullable: false),
                        ProvidendSpendAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BucketCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CampaignBudgetId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CampaignBudgets");
        }
    }
}
