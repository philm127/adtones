// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CampaignProfilePressConfiguration.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Data.Entity.ModelConfiguration;
using EFMVC.Model;

/// <summary>
/// The Configurations namespace.
/// </summary>

namespace EFMVC.Data.Configurations
{
    /// <summary>
    /// Class CampaignProfilePressConfiguration.
    /// </summary>
    public class CampaignProfilePressConfiguration : EntityTypeConfiguration<CampaignProfilePress>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfilePressConfiguration"/> class.
        /// </summary>
        public CampaignProfilePressConfiguration()
        {
            ToTable("CampaignProfilePress");
            Property(u => u.CampaignProfileId).IsRequired();
            Property(u => u.Local);
            Property(u => u.National);
            Property(u => u.FreeNewpapers);
            Property(u => u.Magazines);
        }
    }
}