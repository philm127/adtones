namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPromotionalUserIdFieldinSpentBucket1To10andPromotionalCampaignIdFieldinSpentBucketItem1To10Table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpentBucket10", "PromotionalUserId", c => c.Int());
            AddColumn("dbo.SpentBucket2", "PromotionalUserId", c => c.Int());
            AddColumn("dbo.SpentBucket3", "PromotionalUserId", c => c.Int());
            AddColumn("dbo.SpentBucket4", "PromotionalUserId", c => c.Int());
            AddColumn("dbo.SpentBucket5", "PromotionalUserId", c => c.Int());
            AddColumn("dbo.SpentBucket6", "PromotionalUserId", c => c.Int());
            AddColumn("dbo.SpentBucket7", "PromotionalUserId", c => c.Int());
            AddColumn("dbo.SpentBucket8", "PromotionalUserId", c => c.Int());
            AddColumn("dbo.SpentBucket9", "PromotionalUserId", c => c.Int());
            AddColumn("dbo.SpentBucketItem10", "PromotionalCampaignId", c => c.Int());
            AddColumn("dbo.SpentBucketItem2", "PromotionalCampaignId", c => c.Int());
            AddColumn("dbo.SpentBucketItem3", "PromotionalCampaignId", c => c.Int());
            AddColumn("dbo.SpentBucketItem4", "PromotionalCampaignId", c => c.Int());
            AddColumn("dbo.SpentBucketItem5", "PromotionalCampaignId", c => c.Int());
            AddColumn("dbo.SpentBucketItem6", "PromotionalCampaignId", c => c.Int());
            AddColumn("dbo.SpentBucketItem7", "PromotionalCampaignId", c => c.Int());
            AddColumn("dbo.SpentBucketItem8", "PromotionalCampaignId", c => c.Int());
            AddColumn("dbo.SpentBucketItem9", "PromotionalCampaignId", c => c.Int());
            AddColumn("dbo.SpentBucketItems", "PromotionalCampaignId", c => c.Int());
            AddColumn("dbo.SpentBuckets", "PromotionalUserId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SpentBuckets", "PromotionalUserId");
            DropColumn("dbo.SpentBucketItems", "PromotionalCampaignId");
            DropColumn("dbo.SpentBucketItem9", "PromotionalCampaignId");
            DropColumn("dbo.SpentBucketItem8", "PromotionalCampaignId");
            DropColumn("dbo.SpentBucketItem7", "PromotionalCampaignId");
            DropColumn("dbo.SpentBucketItem6", "PromotionalCampaignId");
            DropColumn("dbo.SpentBucketItem5", "PromotionalCampaignId");
            DropColumn("dbo.SpentBucketItem4", "PromotionalCampaignId");
            DropColumn("dbo.SpentBucketItem3", "PromotionalCampaignId");
            DropColumn("dbo.SpentBucketItem2", "PromotionalCampaignId");
            DropColumn("dbo.SpentBucketItem10", "PromotionalCampaignId");
            DropColumn("dbo.SpentBucket9", "PromotionalUserId");
            DropColumn("dbo.SpentBucket8", "PromotionalUserId");
            DropColumn("dbo.SpentBucket7", "PromotionalUserId");
            DropColumn("dbo.SpentBucket6", "PromotionalUserId");
            DropColumn("dbo.SpentBucket5", "PromotionalUserId");
            DropColumn("dbo.SpentBucket4", "PromotionalUserId");
            DropColumn("dbo.SpentBucket3", "PromotionalUserId");
            DropColumn("dbo.SpentBucket2", "PromotionalUserId");
            DropColumn("dbo.SpentBucket10", "PromotionalUserId");
        }
    }
}
