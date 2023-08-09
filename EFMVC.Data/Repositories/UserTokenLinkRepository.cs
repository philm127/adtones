// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="UserTokenLinkRepository.cs" company="Noat">
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
    /// Class UserTokenLinkRepository.
    /// </summary>
    public class UserTokenLinkRepository : RepositoryBase<UserTokenLink>, IUserTokenLinkRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserTokenLinkRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public UserTokenLinkRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    /// <summary>
    /// Interface IUserTokenLinkRepository
    /// </summary>
    public interface IUserTokenLinkRepository : IRepository<UserTokenLink>
    {
    }
}