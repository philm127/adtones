namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdtoneServerPromotionalCampaignIdandAdtoneServerPromotionalAdvertIdFieldinPromotionalCampaignandPromotionalAdvertTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromotionalAdverts", "AdtoneServerPromotionalAdvertId", c => c.Int());
            AddColumn("dbo.PromotionalCampaigns", "AdtoneServerPromotionalCampaignId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromotionalCampaigns", "AdtoneServerPromotionalCampaignId");
            DropColumn("dbo.PromotionalAdverts", "AdtoneServerPromotionalAdvertId");
        }
    }
}
