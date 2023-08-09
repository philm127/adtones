// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-08-2013
// ***********************************************************************
// <copyright file="CampaignProfileDemographicsRepository.cs" company="">
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
    /// Interface ICampaignProfileDemographicsRepository
    /// </summary>
    public interface ICampaignProfileDemographicsRepository : IRepository<CampaignProfileDemographics>
    {
    }

    /// <summary>
    /// Class CampaignProfileDemographicsRepository.
    /// </summary>
    public class CampaignProfileDemographicsRepository : RepositoryBase<CampaignProfileDemographics>,
                                                         ICampaignProfileDemographicsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfileDemographicsRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public CampaignProfileDemographicsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}