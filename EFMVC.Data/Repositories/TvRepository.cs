// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="TvRepository.cs" company="Noat">
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
    /// Interface ITvRepository
    /// </summary>
    public interface ITvRepository : IRepository<UserProfileTv>
    {
    }

    /// <summary>
    /// Class TvRepository.
    /// </summary>
    public class TvRepository : RepositoryBase<UserProfileTv>, ITvRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TvRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public TvRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}