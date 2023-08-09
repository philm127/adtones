// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-09-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-09-2013
// ***********************************************************************
// <copyright file="BucketAuditRepository.cs" company="">
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
    /// Interface IBucketAuditRepository
    /// </summary>
    public interface IBucketAuditRepository : IRepository<BucketAudit>
    {
    }

    /// <summary>
    /// Class BucketAuditRepository.
    /// </summary>
    public class BucketAuditRepository : RepositoryBase<BucketAudit>, IBucketAuditRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BucketAuditRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public BucketAuditRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}