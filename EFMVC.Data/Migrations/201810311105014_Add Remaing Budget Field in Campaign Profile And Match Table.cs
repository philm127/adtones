namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRemaingBudgetFieldinCampaignProfileAndMatchTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CampaignProfile", "RemainingMaxMonthBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignProfile", "RemainingMaxDailyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignProfile", "RemainingMaxWeeklyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignProfile", "RemainingMaxHourlyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatches", "RemainingMaxMonthBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatches", "RemainingMaxDailyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatches", "RemainingMaxWeeklyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatches", "RemainingMaxHourlyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch10", "RemainingMaxMonthBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch10", "RemainingMaxDailyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch10", "RemainingMaxWeeklyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch10", "RemainingMaxHourlyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch2", "RemainingMaxMonthBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch2", "RemainingMaxDailyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch2", "RemainingMaxWeeklyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch2", "RemainingMaxHourlyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch3", "RemainingMaxMonthBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch3", "RemainingMaxDailyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch3", "RemainingMaxWeeklyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch3", "RemainingMaxHourlyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch4", "RemainingMaxMonthBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch4", "RemainingMaxDailyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch4", "RemainingMaxWeeklyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch4", "RemainingMaxHourlyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch5", "RemainingMaxMonthBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch5", "RemainingMaxDailyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch5", "RemainingMaxWeeklyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch5", "RemainingMaxHourlyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch6", "RemainingMaxMonthBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch6", "RemainingMaxDailyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch6", "RemainingMaxWeeklyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch6", "RemainingMaxHourlyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch7", "RemainingMaxMonthBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch7", "RemainingMaxDailyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch7", "RemainingMaxWeeklyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch7", "RemainingMaxHourlyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch8", "RemainingMaxMonthBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch8", "RemainingMaxDailyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch8", "RemainingMaxWeeklyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch8", "RemainingMaxHourlyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch9", "RemainingMaxMonthBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch9", "RemainingMaxDailyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch9", "RemainingMaxWeeklyBudget", c => c.Single(nullable: false));
            AddColumn("dbo.CampaignMatch9", "RemainingMaxHourlyBudget", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CampaignMatch9", "RemainingMaxHourlyBudget");
            DropColumn("dbo.CampaignMatch9", "RemainingMaxWeeklyBudget");
            DropColumn("dbo.CampaignMatch9", "RemainingMaxDailyBudget");
            DropColumn("dbo.CampaignMatch9", "RemainingMaxMonthBudget");
            DropColumn("dbo.CampaignMatch8", "RemainingMaxHourlyBudget");
            DropColumn("dbo.CampaignMatch8", "RemainingMaxWeeklyBudget");
            DropColumn("dbo.CampaignMatch8", "RemainingMaxDailyBudget");
            DropColumn("dbo.CampaignMatch8", "RemainingMaxMonthBudget");
            DropColumn("dbo.CampaignMatch7", "RemainingMaxHourlyBudget");
            DropColumn("dbo.CampaignMatch7", "RemainingMaxWeeklyBudget");
            DropColumn("dbo.CampaignMatch7", "RemainingMaxDailyBudget");
            DropColumn("dbo.CampaignMatch7", "RemainingMaxMonthBudget");
            DropColumn("dbo.CampaignMatch6", "RemainingMaxHourlyBudget");
            DropColumn("dbo.CampaignMatch6", "RemainingMaxWeeklyBudget");
            DropColumn("dbo.CampaignMatch6", "RemainingMaxDailyBudget");
            DropColumn("dbo.CampaignMatch6", "RemainingMaxMonthBudget");
            DropColumn("dbo.CampaignMatch5", "RemainingMaxHourlyBudget");
            DropColumn("dbo.CampaignMatch5", "RemainingMaxWeeklyBudget");
            DropColumn("dbo.CampaignMatch5", "RemainingMaxDailyBudget");
            DropColumn("dbo.CampaignMatch5", "RemainingMaxMonthBudget");
            DropColumn("dbo.CampaignMatch4", "RemainingMaxHourlyBudget");
            DropColumn("dbo.CampaignMatch4", "RemainingMaxWeeklyBudget");
            DropColumn("dbo.CampaignMatch4", "RemainingMaxDailyBudget");
            DropColumn("dbo.CampaignMatch4", "RemainingMaxMonthBudget");
            DropColumn("dbo.CampaignMatch3", "RemainingMaxHourlyBudget");
            DropColumn("dbo.CampaignMatch3", "RemainingMaxWeeklyBudget");
            DropColumn("dbo.CampaignMatch3", "RemainingMaxDailyBudget");
            DropColumn("dbo.CampaignMatch3", "RemainingMaxMonthBudget");
            DropColumn("dbo.CampaignMatch2", "RemainingMaxHourlyBudget");
            DropColumn("dbo.CampaignMatch2", "RemainingMaxWeeklyBudget");
            DropColumn("dbo.CampaignMatch2", "RemainingMaxDailyBudget");
            DropColumn("dbo.CampaignMatch2", "RemainingMaxMonthBudget");
            DropColumn("dbo.CampaignMatch10", "RemainingMaxHourlyBudget");
            DropColumn("dbo.CampaignMatch10", "RemainingMaxWeeklyBudget");
            DropColumn("dbo.CampaignMatch10", "RemainingMaxDailyBudget");
            DropColumn("dbo.CampaignMatch10", "RemainingMaxMonthBudget");
            DropColumn("dbo.CampaignMatches", "RemainingMaxHourlyBudget");
            DropColumn("dbo.CampaignMatches", "RemainingMaxWeeklyBudget");
            DropColumn("dbo.CampaignMatches", "RemainingMaxDailyBudget");
            DropColumn("dbo.CampaignMatches", "RemainingMaxMonthBudget");
            DropColumn("dbo.CampaignProfile", "RemainingMaxHourlyBudget");
            DropColumn("dbo.CampaignProfile", "RemainingMaxWeeklyBudget");
            DropColumn("dbo.CampaignProfile", "RemainingMaxDailyBudget");
            DropColumn("dbo.CampaignProfile", "RemainingMaxMonthBudget");
        }
    }
}
