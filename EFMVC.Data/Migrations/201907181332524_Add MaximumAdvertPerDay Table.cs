namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMaximumAdvertPerDayTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MaximumAdvertPerDays",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserProfileId = c.Int(nullable: false),
                        RemainingAdvert = c.Int(nullable: false),
                        AddedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MaximumAdvertPerDays");
        }
    }
}
