// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CampaignProfileCinemaConfiguration.cs" company="Noat">
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
    /// Class CampaignProfileCinemaConfiguration.
    /// </summary>
    public class CampaignProfileCinemaConfiguration : EntityTypeConfiguration<CampaignProfileCinema>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfileCinemaConfiguration"/> class.
        /// </summary>
        public CampaignProfileCinemaConfiguration()
        {
            ToTable("CampaignProfileCinema");
            Property(u => u.CampaignProfileId).IsRequired();
            Property(u => u.Cinema);
        }
    }
}