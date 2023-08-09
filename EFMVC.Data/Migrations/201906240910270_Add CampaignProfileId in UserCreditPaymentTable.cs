namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCampaignProfileIdinUserCreditPaymentTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UsersCreditPayment", "CampaignProfileId", c => c.Int());
            CreateIndex("dbo.UsersCreditPayment", "CampaignProfileId");
            AddForeignKey("dbo.UsersCreditPayment", "CampaignProfileId", "dbo.CampaignProfile", "CampaignProfileId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsersCreditPayment", "CampaignProfileId", "dbo.CampaignProfile");
            DropIndex("dbo.UsersCreditPayment", new[] { "CampaignProfileId" });
            DropColumn("dbo.UsersCreditPayment", "CampaignProfileId");
        }
    }
}
