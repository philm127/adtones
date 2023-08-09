// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="UserProfileTvConfiguration.cs" company="Noat">
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
    /// Class UserProfileTvConfiguration.
    /// </summary>
    public class UserProfileTvConfiguration : EntityTypeConfiguration<UserProfileTv>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileTvConfiguration"/> class.
        /// </summary>
        public UserProfileTvConfiguration()
        {
            ToTable("UserProfileTv");
            Property(u => u.UserProfileId).IsRequired();
            Property(u => u.Satallite);
            Property(u => u.Cable);
            Property(u => u.Terrestrial);
            Property(u => u.Internet);
        }
    }
}