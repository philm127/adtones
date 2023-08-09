// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="UserRepository.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.Data.Infrastructure;
using EFMVC.Model;
using EFMVC.Model.Entities;

/// <summary>
/// The Repositories namespace.
/// </summary>

namespace EFMVC.Data.Repositories
{
    /// <summary>
    /// Class UserRepository.
    /// </summary>
    public class UserPasswordHistoryRepository : RepositoryBase<UserPasswordHistory>, IUserPasswordHistoryRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserPasswordHistoryRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public UserPasswordHistoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    /// <summary>
    /// Interface IUserPasswordHistoryRepository
    /// </summary>
    public interface IUserPasswordHistoryRepository : IRepository<UserPasswordHistory>
    {
    }
}