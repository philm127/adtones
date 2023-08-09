// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="InternetRepository.cs" company="Noat">
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
    /// Interface IInternetRepository
    /// </summary>
    public interface IInternetRepository : IRepository<UserProfileInternet>
    {
    }

    /// <summary>
    /// Class InternetRepository.
    /// </summary>
    public class InternetRepository : RepositoryBase<UserProfileInternet>, IInternetRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InternetRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public InternetRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}