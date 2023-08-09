namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLockOutTimefieldinUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "LockOutTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "LockOutTime");
        }
    }
}
