// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="AttitudeRepository.cs" company="">
//     Copyright (c) . All rights reserved.
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
    /// Interface IAttitudeRepository
    /// </summary>
    public interface IAttitudeRepository : IRepository<UserProfileAttitude>
    {
    }

    /// <summary>
    /// Class AttitudeRepository.
    /// </summary>
    public class AttitudeRepository : RepositoryBase<UserProfileAttitude>, IAttitudeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttitudeRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public AttitudeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}