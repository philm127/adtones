// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CinemaRepository.cs" company="Noat">
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
    /// Interface ICinemaRepository
    /// </summary>
    public interface ICinemaRepository : IRepository<UserProfileCinema>
    {
    }

    /// <summary>
    /// Class CinemaRepository.
    /// </summary>
    public class CinemaRepository : RepositoryBase<UserProfileCinema>, ICinemaRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CinemaRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public CinemaRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}