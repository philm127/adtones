// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="UserProfileRadioConfiguration.cs" company="Noat">
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
    /// Class UserProfileRadioConfiguration.
    /// </summary>
    public class UserProfileRadioConfiguration : EntityTypeConfiguration<UserProfileRadio>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileRadioConfiguration"/> class.
        /// </summary>
        public UserProfileRadioConfiguration()
        {
            ToTable("UserProfileRadio");
            Property(u => u.UserProfileId).IsRequired();
            Property(u => u.National);
            Property(u => u.Local);
            Property(u => u.Music);
            Property(u => u.Sport);
            Property(u => u.Talk);
        }
    }
}