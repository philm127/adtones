// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 11-08-2013
// ***********************************************************************
// <copyright file="IRepository.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

/// <summary>
/// The Infrastructure namespace.
/// </summary>

namespace EFMVC.Data.Infrastructure
{
    /// <summary>
    /// Interface IRepository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Add(T entity);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update(T entity);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(T entity);

        /// <summary>
        /// Deletes the specified where.
        /// </summary>
        /// <param name="where">The where.</param>
        void Delete(Expression<Func<T, bool>> where);

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>`0.</returns>
        T GetById(long Id);

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>`0.</returns>
        T GetById(string Id);

        /// <summary>
        /// Gets the specified where.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <returns>`0.</returns>
        T Get(Expression<Func<T, bool>> where);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>IEnumerable{`0}.</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Gets the many.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <returns>IEnumerable{`0}.</returns>
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);

        IQueryable<T> AsQueryable();

        /// <summary>
        /// Count
        /// </summary>
        /// <param name="where">The where</param>
        /// <returns>The count</returns>
        int Count(Expression<Func<T, bool>> where);
    }
}