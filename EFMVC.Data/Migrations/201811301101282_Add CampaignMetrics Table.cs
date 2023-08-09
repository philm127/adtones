namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCampaignMetricsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CampaignMetrics",
                c => new
                    {
                        CampaignMetricsId = c.Int(nullable: false, identity: true),
                        PlaystoDate = c.Single(nullable: false),
                        SpendToDate = c.Double(nullable: false),
                        SMSCost = c.Double(nullable: false),
                        EmailCost = c.Double(nullable: false),
                        AverageBid = c.Double(nullable: false),
                        TotalPlayed = c.Int(nullable: false),
                        MaxBid = c.Double(nullable: false),
                        AverageMaxBid = c.Double(nullable: false),
                        TotalBudget = c.Single(nullable: false),
                        FreePlays = c.Int(nullable: false),
                        AveragePlayTime = c.Double(nullable: false),
                        MaxPlayLength = c.Double(nullable: false),
                        MaxPlayLengthPercantage = c.Double(nullable: false),
                        MaxBidPercantage = c.Double(nullable: false),
                        SMSCampaignCost = c.Double(nullable: false),
                        EmailCampaignCost = c.Double(nullable: false),
                        CancelledCampaignCost = c.Double(nullable: false),
                        Cancelled = c.Double(nullable: false),
                        TotalBudgetPercantage = c.Double(nullable: false),
                        FreePlaysPercantage = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.CampaignMetricsId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CampaignMetrics");
        }
    }
}
