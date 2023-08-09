// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 11-15-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 11-15-2013
// ***********************************************************************
// <copyright file="201311151633520_AddNameColumnToBlockedNumber.cs" company="">
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
    /// Class AddNameColumnToBlockedNumber.
    /// </summary>
    public partial class AddNameColumnToBlockedNumber : DbMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            AddColumn("dbo.BlockedNumbers", "Name", c => c.String());
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            DropColumn("dbo.BlockedNumbers", "Name");
        }
    }
}