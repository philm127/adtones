// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-12-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-12-2014
// ***********************************************************************
// <copyright file="201405121354231_Demographics.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
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
    /// Class Demographics.
    /// </summary>
    public partial class Demographics : DbMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            AddColumn("dbo.CampaignProfileDemographics", "Age", c => c.String(maxLength: 50));
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            DropColumn("dbo.CampaignProfileDemographics", "Age");
        }
    }
}