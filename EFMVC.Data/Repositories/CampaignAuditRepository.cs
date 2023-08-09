// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 01-03-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 01-03-2014
// ***********************************************************************
// <copyright file="CampaignAuditRepository.cs" company="">
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
    /// Interface ICampaignAuditRepository
    /// </summary>
    public interface ICampaignAuditRepository : IRepository<CampaignAudit>
    {
    }

    /// <summary>
    /// Class CampaignAuditRepository.
    /// </summary>
    public class CampaignAuditRepository : RepositoryBase<CampaignAudit>, ICampaignAuditRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignAuditRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public CampaignAuditRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}