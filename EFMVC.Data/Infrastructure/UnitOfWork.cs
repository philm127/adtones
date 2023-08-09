// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 11-08-2013
// ***********************************************************************
// <copyright file="UnitOfWork.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

 /// <summary>
/// The Infrastructure namespace.
/// </summary>

namespace EFMVC.Data.Infrastructure
{
    /// <summary>
    /// Class UnitOfWork.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The database factory
        /// </summary>
        private readonly IDatabaseFactory databaseFactory;

        /// <summary>
        /// The data context
        /// </summary>
        private EFMVCDataContex dataContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            this.databaseFactory = databaseFactory;
        }


        /// <summary>
        /// Gets the data context.
        /// </summary>
        /// <value>The data context.</value>
        protected EFMVCDataContex DataContext
        {
            get { return dataContext ?? (dataContext = databaseFactory.Get()); }
        }

        #region IUnitOfWork Members

        /// <summary>
        /// Commits this instance.
        /// </summary>
        public void Commit()
        {
            DataContext.Commit();
        }

        #endregion
    }
}