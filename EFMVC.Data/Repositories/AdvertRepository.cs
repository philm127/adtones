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
    public interface IUserProfileAdvertRepository : IRepository<UserProfileAdvert>
    {
    }

    /// <summary>
    /// Class UserProfileAdvertRepository.
    /// </summary>
    public class UserProfileAdvertRepository : RepositoryBase<UserProfileAdvert>, IUserProfileAdvertRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileAdvertRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public UserProfileAdvertRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}