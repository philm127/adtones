namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDTMFKeyFieldtoStringinPromotionalCampaignAuditTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PromotionalCampaignAudits", "DTMFKey", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PromotionalCampaignAudits", "DTMFKey", c => c.Int(nullable: false));
        }
    }
}
