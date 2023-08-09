// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="MobileRepository.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.Data.Infrastructure;
using EFMVC.Model;

/// <summary>
/// The Repositories namespace.
/// </summary>

namespace EFMVC.Data.Repositories
{
    /// <summary>
    /// Interface IMobileRepository
    /// </summary>
    public interface IMobileRepository : IRepository<UserProfileMobile>
    {
    }

    /// <summary>
    /// Class MobileRepository.
    /// </summary>
    public class MobileRepository : RepositoryBase<UserProfileMobile>, IMobileRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MobileRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public MobileRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}