// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="AdvertRepository.cs" company="">
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
    /// Interface IUserProfileAdvertRepository
    /// </summary>
    public interface IAdvertRejectionRepository : IRepository<AdvertRejection>
    {
    }

    /// <summary>
    /// Class UserProfileAdvertRepository.
    /// </summary>
    public class AdvertRejectionRepository : RepositoryBase<AdvertRejection>, IAdvertRejectionRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdvertRejectionRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public AdvertRejectionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}