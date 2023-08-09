namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProvitionUserTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProvitionUsers",
                c => new
                    {
                        ProvitionUserID = c.Int(nullable: false, identity: true),
                        PromotionalUserId = c.Int(),
                        UserProfileId = c.Int(),
                        MSISDN = c.String(),
                        DTMFKey = c.Int(),
                    })
                .PrimaryKey(t => t.ProvitionUserID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProvitionUsers");
        }
    }
}
