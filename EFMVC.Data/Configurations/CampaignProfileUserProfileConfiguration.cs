// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CampaignProfileUserProfileConfiguration.cs" company="Noat">
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
    /// Class CampaignProfileUserProfileConfiguration.
    /// </summary>
    public class CampaignProfileUserProfileConfiguration : EntityTypeConfiguration<CampaignProfileUserProfile>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfileUserProfileConfiguration"/> class.
        /// </summary>
        public CampaignProfileUserProfileConfiguration()
        {
            ToTable("CampaignProfileUserProfile");
            HasKey(x => x.Id);
            Property(x => x.Id).IsRequired();
            Property(a => a.CampaignProfileId).IsRequired();
            Property(a => a.UserProfileId).IsRequired();
        }
    }
}