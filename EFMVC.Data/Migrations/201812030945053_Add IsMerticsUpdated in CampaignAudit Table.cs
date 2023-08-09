namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsMerticsUpdatedinCampaignAuditTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CampaignAudit", "IsMerticsUpdated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CampaignAudit", "IsMerticsUpdated");
        }
    }
}
