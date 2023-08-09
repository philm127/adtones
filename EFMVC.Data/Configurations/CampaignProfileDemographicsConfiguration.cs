// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-22-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-22-2014
// ***********************************************************************
// <copyright file="CampaignProfileDemographicsConfiguration.cs" company="Noat">
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
    /// Class CampaignProfileDemographicsConfiguration.
    /// </summary>
    public class CampaignProfileDemographicsConfiguration : EntityTypeConfiguration<CampaignProfileDemographics>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfileDemographicsConfiguration"/> class.
        /// </summary>
        public CampaignProfileDemographicsConfiguration()
        {
            HasKey(d => d.CampaignProfileDemographicsId);
            Property(d => d.CampaignProfileDemographicsId).IsRequired();
            Property(d => d.CampaignProfileId).IsRequired();
            Property(u => u.DOBStart);
            Property(u => u.DOBEnd);
            Property(u => u.Gender).HasMaxLength(50);
            Property(u => u.IncomeBracket).HasMaxLength(50);
            Property(u => u.WorkingStatus).HasMaxLength(50);
            Property(u => u.RelationshipStatus).HasMaxLength(50);
            Property(u => u.Education).HasMaxLength(50);
            Property(u => u.HouseholdStatus).HasMaxLength(50);
            Property(u => u.Location).HasMaxLength(50);
        }
    }
}