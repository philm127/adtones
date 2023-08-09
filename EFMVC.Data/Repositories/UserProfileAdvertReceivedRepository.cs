// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="UserProfileAdvertRepository.cs" company="Noat">
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
    /// Interface IUserProfileAdvertsReceivedRepository
    /// </summary>
    public interface IUserProfileAdvertsReceivedRepository : IRepository<UserProfileAdvertsReceived>
    {
    }

    /// <summary>
    /// Class UserProfileAdvertsReceivedRepository.
    /// </summary>
    public class UserProfileAdvertsReceivedRepository : RepositoryBase<UserProfileAdvertsReceived>, IUserProfileAdvertsReceivedRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileAdvertsReceivedRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public UserProfileAdvertsReceivedRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}