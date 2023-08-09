// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="UserProfileTimeSettingConfiguration.cs" company="Noat">
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
    /// Class UserProfileTimeSettingConfiguration.
    /// </summary>
    public class UserProfileTimeSettingConfiguration : EntityTypeConfiguration<UserProfileTimeSetting>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileTimeSettingConfiguration"/> class.
        /// </summary>
        public UserProfileTimeSettingConfiguration()
        {
            ToTable("UserProfileTimeSetting");
            Property(u => u.UserProfileId).IsRequired();
            Property(u => u.Monday).HasMaxLength(200);
            Property(u => u.Tuesday).HasMaxLength(200);
            Property(u => u.Wednesday).HasMaxLength(200);
            Property(u => u.Thursday).HasMaxLength(200);
            Property(u => u.Friday).HasMaxLength(200);
            Property(u => u.Saturday).HasMaxLength(200);
            Property(u => u.Sunday).HasMaxLength(200);
        }
    }
}