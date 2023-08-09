// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="BlockedNumbersRepository.cs" company="">
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
    /// Interface IBlockedNumberRepository
    /// </summary>
    public interface IBlockedNumberRepository : IRepository<BlockedNumber>
    {
    }

    /// <summary>
    /// Class BlockedNumbersRepository.
    /// </summary>
    public class BlockedNumbersRepository : RepositoryBase<BlockedNumber>, IBlockedNumberRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlockedNumbersRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public BlockedNumbersRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}