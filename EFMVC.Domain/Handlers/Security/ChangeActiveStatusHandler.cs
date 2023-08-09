// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="ChangePasswordHandler.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.CommandProcessor.Command;
using EFMVC.Data;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Model;
using System.Linq;
/// <summary>
/// The Security namespace.
/// </summary>

namespace EFMVC.Domain.Handlers.Security
{
    /// <summary>
    /// Class ChangePasswordHandler.
    /// </summary>
    public class ChangeActiveStatusHandler : ICommandHandler<ChangeActiveStatusCommand>
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;
        private readonly IContactsRepository contactsRepository;
        /// <summary>
        /// The user repository
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangePasswordHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public ChangeActiveStatusHandler(IUserRepository userRepository, IContactsRepository contactsRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.contactsRepository = contactsRepository;
            this.unitOfWork = unitOfWork;
        }

        #region ICommandHandler<ChangeActiveStatusCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(ChangeActiveStatusCommand command)
        {
            User user = userRepository.GetById(command.UserId);
            user.Activated = command.Activated;
            userRepository.Update(user);
            unitOfWork.Commit();

            if(user.RoleId == 2)
            {
                var ConnString = ConnectionString.GetConnectionStringByOperatorId(user.OperatorId);
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var userData = db.Users.Where(s => s.AdtoneServerUserId == command.UserId).FirstOrDefault();
                        if (userData != null)
                        {
                            userData.Activated = command.Activated;
                            db.SaveChanges();
                        }
                    }
                }
            }
            else if(user.RoleId == 3)
            {
                var contactData = contactsRepository.GetMany(s => s.UserId == user.UserId).FirstOrDefault();
                if(contactData != null && contactData.CountryId != null)
                {
                    var ConnString = ConnectionString.GetConnectionStringByCountryId(contactData.CountryId);
                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        foreach (var item in ConnString)
                        {
                            EFMVCDataContex db = new EFMVCDataContex(item);
                            var userData = db.Users.Where(s => s.AdtoneServerUserId == command.UserId).FirstOrDefault();
                            if (userData != null)
                            {
                                userData.Activated = command.Activated;
                                db.SaveChanges();
                            }
                        }
                    }
                }
                
            }
            
                return new CommandResult(true);
        }

        #endregion
    }
}