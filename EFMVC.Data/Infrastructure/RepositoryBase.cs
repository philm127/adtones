// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 11-08-2013
// ***********************************************************************
// <copyright file="RepositoryBase.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

/// <summary>
/// The Infrastructure namespace.
/// </summary>

namespace EFMVC.Data.Infrastructure
{
    /// <summary>
    /// Class RepositoryBase.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryBase<T> where T : class
    {
        /// <summary>
        /// The dbset
        /// </summary>
        private readonly IDbSet<T> dbset;

        /// <summary>
        /// The data context
        /// </summary>
        private EFMVCDataContex dataContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{T}" /> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            dbset = DataContext.Set<T>();
        }

        /// <summary>
        /// Gets the database factory.
        /// </summary>
        /// <value>The database factory.</value>
        protected IDatabaseFactory DatabaseFactory { get; private set; }

        /// <summary>
        /// Gets the data context.
        /// </summary>
        /// <value>The data context.</value>
        protected EFMVCDataContex DataContext
        {
            get { return dataContext ?? (dataContext = DatabaseFactory.Get()); }
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Add(T entity)
        {
            dbset.Add(entity);
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">Entity Validation Failed - errors follow:\n +
        /// sb.ToString()</exception>
        public virtual void Update(T entity)
        {
            try
            {
                dbset.Attach(entity);
                dataContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();

                foreach (DbEntityValidationResult failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (DbValidationError error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb, ex
                    ); // Add the original exception as the innerException
            }
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Delete(T entity)
        {
            dbset.Remove(entity);
        }

        /// <summary>
        /// Deletes the specified where.
        /// </summary>
        /// <param name="where">The where.</param>
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbset.Where(where).AsEnumerable();
            foreach (T obj in objects)
                dbset.Remove(obj);
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>`0.</returns>
        public virtual T GetById(long id)
        {
            return dbset.Find(id);
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>`0.</returns>
        public virtual T GetById(string id)
        {
            return dbset.Find(id);
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>IEnumerable{`0}.</returns>
        public virtual IEnumerable<T> GetAll()
        {
            return dbset.ToList();
        }

        /// <summary>
        /// Gets the many.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <returns>IEnumerable{`0}.</returns>
        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where).ToList();
        }

        /// <summary>
        /// Gets the specified where.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <returns>`0.</returns>
        public T Get(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where).FirstOrDefault();
        }

        public int Count(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where).Count();
        }

        public IQueryable<T> AsQueryable()
        {
            return dbset;
        }
    }
}