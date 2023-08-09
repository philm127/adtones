namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFlotChartMetricsandSparkLineMetricsTableandAddUserIdinCampaignMetricsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FlotChartMetrics",
                c => new
                    {
                        FlotChartMetricsId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        NoofPlayName = c.Int(nullable: false),
                        NoofPlayValue = c.Int(nullable: false),
                        AvgBidName = c.Int(nullable: false),
                        AvgBidValue = c.Int(nullable: false),
                        NoofplayMaxCount = c.Int(nullable: false),
                        AvgbidMaxCount = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FlotChartMetricsId);
            
            CreateTable(
                "dbo.SparkLineMetrics",
                c => new
                    {
                        SparkLineMetricsId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Second = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SparkLineMetricsId);
            
            AddColumn("dbo.CampaignMetrics", "AvgMaxBid", c => c.Double(nullable: false));
            AddColumn("dbo.CampaignMetrics", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.CampaignMetrics", "AverageMaxBid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CampaignMetrics", "AverageMaxBid", c => c.Double(nullable: false));
            DropColumn("dbo.CampaignMetrics", "UserId");
            DropColumn("dbo.CampaignMetrics", "AvgMaxBid");
            DropTable("dbo.SparkLineMetrics");
            DropTable("dbo.FlotChartMetrics");
        }
    }
}
