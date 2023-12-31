namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserTokenLinkTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserTokenLinks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        UserToken = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserTokenLinks", "UserId", "dbo.Users");
            DropIndex("dbo.UserTokenLinks", new[] { "UserId" });
            DropTable("dbo.UserTokenLinks");
        }
    }
}
