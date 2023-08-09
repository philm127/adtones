namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRankNumberinPreMatchandPreMatch2Table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PreMatches", "RankNumber", c => c.Int(nullable: false));
            AddColumn("dbo.PreMatch2", "RankNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PreMatch2", "RankNumber");
            DropColumn("dbo.PreMatches", "RankNumber");
        }
    }
}
