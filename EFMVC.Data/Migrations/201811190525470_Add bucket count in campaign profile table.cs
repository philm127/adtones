namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addbucketcountincampaignprofiletable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CampaignProfile", "ProvidendSpendAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.CampaignProfile", "BucketCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CampaignProfile", "BucketCount");
            DropColumn("dbo.CampaignProfile", "ProvidendSpendAmount");
        }
    }
}
