namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdvertRejectiontable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdvertRejections",
                c => new
                    {
                        AdvertRejectionId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        AdvertId = c.Int(),
                        RejectionReason = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AdvertRejectionId)
                .ForeignKey("dbo.Advert", t => t.AdvertId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.AdvertId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AdvertRejections", "UserId", "dbo.Users");
            DropForeignKey("dbo.AdvertRejections", "AdvertId", "dbo.Advert");
            DropIndex("dbo.AdvertRejections", new[] { "AdvertId" });
            DropIndex("dbo.AdvertRejections", new[] { "UserId" });
            DropTable("dbo.AdvertRejections");
        }
    }
}
