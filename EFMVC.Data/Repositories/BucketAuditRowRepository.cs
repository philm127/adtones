// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-09-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-09-2013
// ***********************************************************************
// <copyright file="BucketAuditRowRepository.cs" company="">
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
    /// Interface IBucketAuditRowRepository
    /// </summary>
    public interface IBucketAuditRowRepository : IRepository<BucketAuditRow>
    {
    }

    /// <summary>
    /// Class BucketAuditRowRepository.
    /// </summary>
    public class BucketAuditRowRepository : RepositoryBase<BucketAuditRow>, IBucketAuditRowRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BucketAuditRowRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public BucketAuditRowRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}