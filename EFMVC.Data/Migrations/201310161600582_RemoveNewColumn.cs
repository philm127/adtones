namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveNewColumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CampaignProfile", "NewColumn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CampaignProfile", "NewColumn", c => c.String(maxLength: 50));
        }
    }
}
