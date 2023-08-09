// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CampaignProfileTvConfiguration.cs" company="Noat">
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
    /// Class CampaignProfileTvConfiguration.
    /// </summary>
    public class CampaignProfileTvConfiguration : EntityTypeConfiguration<CampaignProfileTv>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfileTvConfiguration"/> class.
        /// </summary>
        public CampaignProfileTvConfiguration()
        {
            ToTable("CampaignProfileTv");
            Property(u => u.CampaignProfileId).IsRequired();
            Property(u => u.Satallite);
            Property(u => u.Cable);
            Property(u => u.Terrestrial);
            Property(u => u.Internet);
        }
    }
}