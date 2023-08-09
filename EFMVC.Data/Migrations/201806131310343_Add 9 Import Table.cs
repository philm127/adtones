namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add9ImportTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Import10",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceCode = c.String(),
                        CallingNumber = c.String(),
                        CalledNumber = c.String(),
                        RBTCode = c.String(),
                        SPCode = c.String(),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        CallScheme = c.String(),
                        DTMF = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Import2",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceCode = c.String(),
                        CallingNumber = c.String(),
                        CalledNumber = c.String(),
                        RBTCode = c.String(),
                        SPCode = c.String(),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        CallScheme = c.String(),
                        DTMF = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Import3",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceCode = c.String(),
                        CallingNumber = c.String(),
                        CalledNumber = c.String(),
                        RBTCode = c.String(),
                        SPCode = c.String(),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        CallScheme = c.String(),
                        DTMF = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Import4",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceCode = c.String(),
                        CallingNumber = c.String(),
                        CalledNumber = c.String(),
                        RBTCode = c.String(),
                        SPCode = c.String(),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        CallScheme = c.String(),
                        DTMF = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Import5",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceCode = c.String(),
                        CallingNumber = c.String(),
                        CalledNumber = c.String(),
                        RBTCode = c.String(),
                        SPCode = c.String(),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        CallScheme = c.String(),
                        DTMF = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Import6",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceCode = c.String(),
                        CallingNumber = c.String(),
                        CalledNumber = c.String(),
                        RBTCode = c.String(),
                        SPCode = c.String(),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        CallScheme = c.String(),
                        DTMF = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Import7",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceCode = c.String(),
                        CallingNumber = c.String(),
                        CalledNumber = c.String(),
                        RBTCode = c.String(),
                        SPCode = c.String(),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        CallScheme = c.String(),
                        DTMF = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Import8",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceCode = c.String(),
                        CallingNumber = c.String(),
                        CalledNumber = c.String(),
                        RBTCode = c.String(),
                        SPCode = c.String(),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        CallScheme = c.String(),
                        DTMF = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Import9",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceCode = c.String(),
                        CallingNumber = c.String(),
                        CalledNumber = c.String(),
                        RBTCode = c.String(),
                        SPCode = c.String(),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        CallScheme = c.String(),
                        DTMF = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Import9");
            DropTable("dbo.Import8");
            DropTable("dbo.Import7");
            DropTable("dbo.Import6");
            DropTable("dbo.Import5");
            DropTable("dbo.Import4");
            DropTable("dbo.Import3");
            DropTable("dbo.Import2");
            DropTable("dbo.Import10");
        }
    }
}
