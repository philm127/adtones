namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAddUserTrackTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AddUserTracks",
                c => new
                    {
                        AddUserTrackId = c.Int(nullable: false, identity: true),
                        UserMatchTableName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.AddUserTrackId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AddUserTracks");
        }
    }
}
