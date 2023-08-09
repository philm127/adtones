﻿// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="ImportsRepository.cs" company="Noat">
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
    /// Class ImportsRepository.
    /// </summary>
    public class ImportsRepository : RepositoryBase<Import>, IImportsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportsRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public ImportsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    /// <summary>
    /// Interface IImportsRepository
    /// </summary>
    public interface IImportsRepository : IRepository<Import>
    {
    }
}