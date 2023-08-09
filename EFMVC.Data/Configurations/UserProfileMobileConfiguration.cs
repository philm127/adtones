// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="UserProfileMobileConfiguration.cs" company="Noat">
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
    /// Class UserProfileMobileConfiguration.
    /// </summary>
    public class UserProfileMobileConfiguration : EntityTypeConfiguration<UserProfileMobile>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileMobileConfiguration"/> class.
        /// </summary>
        public UserProfileMobileConfiguration()
        {
            ToTable("UserProfileMobile");
            Property(u => u.UserProfileId).IsRequired();
            Property(u => u.ContractType);
            Property(u => u.Spend);
        }
    }
}