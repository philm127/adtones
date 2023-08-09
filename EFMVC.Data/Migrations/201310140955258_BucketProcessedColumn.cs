// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-14-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-14-2013
// ***********************************************************************
// <copyright file="201310140955258_BucketProcessedColumn.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
/// <summary>
/// The Migrations namespace.
/// </summary>

using System.Data.Entity.Migrations;

namespace EFMVC.Data.Migrations
{
    /// <summary>
    /// Class BucketProcessedColumn.
    /// </summary>
    public partial class BucketProcessedColumn : DbMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            AddColumn("dbo.BucketAudits", "Processed", c => c.Boolean(nullable: false));
            AddColumn("dbo.BucketAuditRows", "Processed", c => c.Boolean(nullable: false));
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            DropColumn("dbo.BucketAuditRows", "Processed");
            DropColumn("dbo.BucketAudits", "Processed");
        }
    }
}