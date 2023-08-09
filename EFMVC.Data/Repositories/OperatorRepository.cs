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
using EFMVC.Model.Entities;

/// <summary>
/// The Repositories namespace.
/// </summary>

namespace EFMVC.Data.Repositories
{
    /// <summary>
    /// Interface IOperatorRepository
    /// </summary>
    public interface IOperatorRepository : IRepository<Operator>
    {
    }

    /// <summary>
    /// Class OperatorRepository.
    /// </summary>
    public class OperatorRepository : RepositoryBase<Operator>, IOperatorRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public OperatorRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}