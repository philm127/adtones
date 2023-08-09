namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNumOfLiveCampaigninImportFileTrackTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImportFileTracks", "NumOfLiveCampaign", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImportFileTracks", "NumOfLiveCampaign");
        }
    }
}
