namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPromotionalUserIdFieldinBucket1To10andPromotionalCampaignIdFieldinBucketItem1To10Table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Buckets", "PromotionalUserId", c => c.Int());
            AddColumn("dbo.Bucket10", "PromotionalUserId", c => c.Int());
            AddColumn("dbo.Bucket2", "PromotionalUserId", c => c.Int());
            AddColumn("dbo.Bucket3", "PromotionalUserId", c => c.Int());
            AddColumn("dbo.Bucket4", "PromotionalUserId", c => c.Int());
            AddColumn("dbo.Bucket5", "PromotionalUserId", c => c.Int());
            AddColumn("dbo.Bucket6", "PromotionalUserId", c => c.Int());
            AddColumn("dbo.Bucket7", "PromotionalUserId", c => c.Int());
            AddColumn("dbo.Bucket8", "PromotionalUserId", c => c.Int());
            AddColumn("dbo.Bucket9", "PromotionalUserId", c => c.Int());
            AddColumn("dbo.BucketItems", "PromotionalCampaignId", c => c.Int());
            AddColumn("dbo.BucketItem10", "PromotionalCampaignId", c => c.Int());
            AddColumn("dbo.BucketItem2", "PromotionalCampaignId", c => c.Int());
            AddColumn("dbo.BucketItem3", "PromotionalCampaignId", c => c.Int());
            AddColumn("dbo.BucketItem4", "PromotionalCampaignId", c => c.Int());
            AddColumn("dbo.BucketItem5", "PromotionalCampaignId", c => c.Int());
            AddColumn("dbo.BucketItem6", "PromotionalCampaignId", c => c.Int());
            AddColumn("dbo.BucketItem7", "PromotionalCampaignId", c => c.Int());
            AddColumn("dbo.BucketItem8", "PromotionalCampaignId", c => c.Int());
            AddColumn("dbo.BucketItem9", "PromotionalCampaignId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BucketItem9", "PromotionalCampaignId");
            DropColumn("dbo.BucketItem8", "PromotionalCampaignId");
            DropColumn("dbo.BucketItem7", "PromotionalCampaignId");
            DropColumn("dbo.BucketItem6", "PromotionalCampaignId");
            DropColumn("dbo.BucketItem5", "PromotionalCampaignId");
            DropColumn("dbo.BucketItem4", "PromotionalCampaignId");
            DropColumn("dbo.BucketItem3", "PromotionalCampaignId");
            DropColumn("dbo.BucketItem2", "PromotionalCampaignId");
            DropColumn("dbo.BucketItem10", "PromotionalCampaignId");
            DropColumn("dbo.BucketItems", "PromotionalCampaignId");
            DropColumn("dbo.Bucket9", "PromotionalUserId");
            DropColumn("dbo.Bucket8", "PromotionalUserId");
            DropColumn("dbo.Bucket7", "PromotionalUserId");
            DropColumn("dbo.Bucket6", "PromotionalUserId");
            DropColumn("dbo.Bucket5", "PromotionalUserId");
            DropColumn("dbo.Bucket4", "PromotionalUserId");
            DropColumn("dbo.Bucket3", "PromotionalUserId");
            DropColumn("dbo.Bucket2", "PromotionalUserId");
            DropColumn("dbo.Bucket10", "PromotionalUserId");
            DropColumn("dbo.Buckets", "PromotionalUserId");
        }
    }
}
