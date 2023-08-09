// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="UserProfilePressConfiguration.cs" company="Noat">
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
    /// Class UserProfilePressConfiguration.
    /// </summary>
    public class UserProfilePressConfiguration : EntityTypeConfiguration<UserProfilePress>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfilePressConfiguration"/> class.
        /// </summary>
        public UserProfilePressConfiguration()
        {
            ToTable("UserProfilePress");
            Property(u => u.UserProfileId).IsRequired();
            Property(u => u.Local);
            Property(u => u.National);
            Property(u => u.FreeNewpapers);
            Property(u => u.Magazines);
        }
    }
}