// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="201405091755314_StringToIntForCampaignQuestions2.cs" company="">
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
    /// Class StringToIntForCampaignQuestions2.
    /// </summary>
    public partial class StringToIntForCampaignQuestions2 : DbMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            AlterColumn("dbo.UserProfileRadio", "National", c => c.Int());
            AlterColumn("dbo.UserProfileRadio", "Local", c => c.Int());
            AlterColumn("dbo.UserProfileRadio", "Music", c => c.Int());
            AlterColumn("dbo.UserProfileRadio", "Sport", c => c.Int());
            AlterColumn("dbo.UserProfileRadio", "Talk", c => c.Int());
            AlterColumn("dbo.UserProfileTv", "Satallite", c => c.Int());
            AlterColumn("dbo.UserProfileTv", "Cable", c => c.Int());
            AlterColumn("dbo.UserProfileTv", "Terrestrial", c => c.Int());
            AlterColumn("dbo.UserProfileTv", "Internet", c => c.Int());
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            AlterColumn("dbo.UserProfileTv", "Internet", c => c.String());
            AlterColumn("dbo.UserProfileTv", "Terrestrial", c => c.String());
            AlterColumn("dbo.UserProfileTv", "Cable", c => c.String());
            AlterColumn("dbo.UserProfileTv", "Satallite", c => c.String());
            AlterColumn("dbo.UserProfileRadio", "Talk", c => c.String());
            AlterColumn("dbo.UserProfileRadio", "Sport", c => c.String());
            AlterColumn("dbo.UserProfileRadio", "Music", c => c.String());
            AlterColumn("dbo.UserProfileRadio", "Local", c => c.String());
            AlterColumn("dbo.UserProfileRadio", "National", c => c.String());
        }
    }
}