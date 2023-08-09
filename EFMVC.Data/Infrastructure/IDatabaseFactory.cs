// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 11-08-2013
// ***********************************************************************
// <copyright file="IDatabaseFactory.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

/// <summary>
/// The Infrastructure namespace.
/// </summary>

namespace EFMVC.Data.Infrastructure
{
    /// <summary>
    /// Interface IDatabaseFactory
    /// </summary>
    public interface IDatabaseFactory : IDisposable
    {
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>EFMVCDataContex.</returns>
        EFMVCDataContex Get();
    }
}