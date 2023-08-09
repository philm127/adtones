namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteRankNumberinPreMatchAndPreMatch2Table : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PreMatches", "RankNumber");
            DropColumn("dbo.PreMatch2", "RankNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PreMatch2", "RankNumber", c => c.Int(nullable: false));
            AddColumn("dbo.PreMatches", "RankNumber", c => c.Int(nullable: false));
        }
    }
}
