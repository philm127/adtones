// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="UserProfileConfiguration.cs" company="Noat">
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
    /// Class UserProfileConfiguration.
    /// </summary>
    public class UserProfileConfiguration : EntityTypeConfiguration<UserProfile>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileConfiguration"/> class.
        /// </summary>
        public UserProfileConfiguration()
        {
            ToTable("UserProfile");
            Property(u => u.UserProfileId).IsRequired();
            Property(u => u.UserId);
            Property(u => u.DOB);
            Property(u => u.Gender).HasMaxLength(50);
            Property(u => u.IncomeBracket).HasMaxLength(50);
            Property(u => u.WorkingStatus).HasMaxLength(50);
            Property(u => u.RelationshipStatus).HasMaxLength(50);
            Property(u => u.Education).HasMaxLength(50);
            Property(u => u.HouseholdStatus).HasMaxLength(50);
            Property(u => u.Location).HasMaxLength(50);
            Property(u => u.MSISDN).HasMaxLength(50);
           //Property(u => u.Postcode).HasMaxLength(100);

            HasKey(x => x.UserProfileId);
        }
    }
}