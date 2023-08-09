// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="PressRepository.cs" company="Noat">
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
    /// Interface IPressRepository
    /// </summary>
    public interface IPressRepository : IRepository<UserProfilePress>
    {
    }

    /// <summary>
    /// Class PressRepository.
    /// </summary>
    public class PressRepository : RepositoryBase<UserProfilePress>, IPressRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PressRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public PressRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}