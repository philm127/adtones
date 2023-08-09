namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNumberInBatchincampaingProfileTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CampaignProfile", "NumberInBatch", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CampaignProfile", "NumberInBatch");
        }
    }
}
