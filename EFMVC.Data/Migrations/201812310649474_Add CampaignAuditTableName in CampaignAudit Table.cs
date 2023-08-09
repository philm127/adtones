namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCampaignAuditTableNameinCampaignAuditTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CampaignAudit", "CampaignAuditTableName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CampaignAudit", "CampaignAuditTableName");
        }
    }
}
