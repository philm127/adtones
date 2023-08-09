namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserMatchTableNameFieldinUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "UserMatchTableName", c => c.String(maxLength: 50));
            DropTable("dbo.AddUserTracks");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AddUserTracks",
                c => new
                    {
                        AddUserTrackId = c.Int(nullable: false, identity: true),
                        UserMatchTableName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.AddUserTrackId);
            
            DropColumn("dbo.Users", "UserMatchTableName");
        }
    }
}
