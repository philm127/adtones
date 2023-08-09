// ***********************************************************************
// Assembly         : EFMVC.EF
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="User_Facade.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The EF namespace.
/// </summary>
namespace EFMVC.EF
{
    /// <summary>
    /// Class User.
    /// </summary>
    public partial class User
    {
        /// <summary>
        /// Gets the get user profile.
        /// </summary>
        /// <value>The get user profile.</value>
        public UserProfile GetUserProfile
        {
            get { return this.UserProfiles.FirstOrDefault(); }
        }
    }
}
