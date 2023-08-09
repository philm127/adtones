namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPromotionalUserIdFieldinMaximumAdvertPerDayTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MaximumAdvertPerDays", "PromotionalUserId", c => c.Int());
            AlterColumn("dbo.MaximumAdvertPerDays", "UserProfileId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MaximumAdvertPerDays", "UserProfileId", c => c.Int(nullable: false));
            DropColumn("dbo.MaximumAdvertPerDays", "PromotionalUserId");
        }
    }
}
