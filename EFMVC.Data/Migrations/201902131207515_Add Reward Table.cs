namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRewardTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rewards",
                c => new
                    {
                        RewardId = c.Int(nullable: false, identity: true),
                        RewardName = c.String(),
                        RewardValue = c.String(),
                        AddedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RewardId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Rewards");
        }
    }
}
