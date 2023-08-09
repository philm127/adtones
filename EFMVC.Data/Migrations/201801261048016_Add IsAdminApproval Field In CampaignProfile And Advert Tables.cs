namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsAdminApprovalFieldInCampaignProfileAndAdvertTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Advert", "IsAdminApproval", c => c.Boolean(nullable: false));
            AddColumn("dbo.CampaignProfile", "IsAdminApproval", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CampaignProfile", "IsAdminApproval");
            DropColumn("dbo.Advert", "IsAdminApproval");
        }
    }
}
