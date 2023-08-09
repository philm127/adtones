namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Adddeliverydetailstopromouser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromotionalUsers", "DeliveryServerConnectionString", c => c.String());
            AddColumn("dbo.PromotionalUsers", "DeliveryServerIpAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromotionalUsers", "DeliveryServerIpAddress");
            DropColumn("dbo.PromotionalUsers", "DeliveryServerConnectionString");
        }
    }
}
